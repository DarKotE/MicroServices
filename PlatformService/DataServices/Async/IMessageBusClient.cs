using PlatformService.Dto;
using PlatformService.Models;

namespace PlatformService.DataServices.Async;

public interface IMessageBusClient
{
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
}