using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dto;
using CommandService.Models;

namespace CommandService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
        var eventStr = JsonSerializer.Deserialize<GenericEventDto>(message);

        switch (eventStr)
        {
            case { Event: "Platform_Published" }:
                ProcessPlatformPublish();
                break;
            default:
                Console.WriteLine("Unknown event");
                break;
        }

        void ProcessPlatformPublish()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
            try
            {
                var platform = _mapper.Map<Platform>(platformPublishedDto);
                if (repo.Exists(platform.OriginId))
                    return;
                repo.CreatePlatform(platform);
                repo.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}



