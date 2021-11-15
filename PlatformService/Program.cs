using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.DataServices.Async;
using PlatformService.DataServices.Sync.Grpc;
using PlatformService.DataServices.Sync.Http;
using PlatformService.Dto;
using PlatformService.Models;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMemory"));

}
else
{
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlatformConn")));
}

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();
builder.Services.AddHttpClient<ICommandClient, HttpCommandClient>();
builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();
builder.Services.AddGrpc();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("api/platforms", (IPlatformRepo repo, IMapper mapper) =>
    Results.Ok(mapper.Map<IEnumerable<PlatformReadDto>>(repo.GetAll())));

app.MapGet("api/platforms/{id}", (IPlatformRepo repo, IMapper mapper, Guid id) =>
    {
        var entry = repo.GetById(id);
        return entry is not null ? Results.Ok(mapper.Map<PlatformReadDto?>(entry)) : Results.NotFound();
    })
    .WithName("GetPlatformById");

app.MapPost("api/platforms", async (
    IPlatformRepo repo,
    IMapper mapper,
    ICommandClient commandClient,
    IMessageBusClient messageBusClient,
    PlatformCreateDto dto
) =>
{
    var platform = mapper.Map<Platform>(dto);
    repo.Create(platform);
    var saved = repo.SaveChanges();
    var returnDto = mapper.Map<PlatformReadDto>(platform);

    try
    {
        await commandClient.SendPlatform(returnDto);
    }
    catch (Exception e)
    {
        Console.WriteLine("Cant send sync command:" + e.Message);
    }
    try
    {
        var publishedDto = mapper.Map<PlatformPublishedDto>(returnDto);
        messageBusClient.PublishNewPlatform(publishedDto with {Event = "Platform_Published"});
    }
    catch (Exception e)
    {
        Console.WriteLine("Cant send async command:" + e.Message);
    }
    return saved
        ? Results.CreatedAtRoute("GetPlatformById", new { platform.Id }, returnDto)
        : Results.StatusCode(StatusCodes.Status500InternalServerError);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<GrpcPlatformService>();

    endpoints.MapGet("/proto/platforms.proto", async context =>
    {
        await context.Response.WriteAsync(File.ReadAllText("Proto/platforms.proto"));
    });
});
//app.UseHttpsRedirection();

TestDb.Populate(app, app.Environment.IsProduction());

app.Run();

