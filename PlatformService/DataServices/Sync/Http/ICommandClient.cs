using PlatformService.Dto;

namespace PlatformService.DataServices.Sync.Http;

public interface ICommandClient
{
    Task SendPlatform(PlatformReadDto platform);
}