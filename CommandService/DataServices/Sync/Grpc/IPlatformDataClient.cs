using CommandService.Models;

namespace CommandService.DataServices.Sync.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();
}