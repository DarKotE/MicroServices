using AutoMapper;
using CommandService.Models;
using Grpc.Net.Client;
using PlatformService;

namespace CommandService.DataServices.Sync.Grpc;

public sealed class PlatformDataClient : IPlatformDataClient
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public PlatformDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }
    
    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        var channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var response = client.GetAllPlatforms(request);
            return _mapper.Map<IEnumerable<Platform>>(response.Platform);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Array.Empty<Platform>();
        }
    }
}