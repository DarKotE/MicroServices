using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dto;

public record PlatformReadDto(Guid Id, string Name, decimal Cost);

public record PlatformCreateDto(
    [property: Required] string Name,
    [property: Required] string Publisher,
    [property: Required] decimal Cost);

public record PlatformPublishedDto(Guid Id, string Name, string Event = "");
