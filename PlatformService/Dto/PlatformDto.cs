using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dto;

public sealed record PlatformReadDto(Guid Id, string Name, decimal Cost);

public sealed record PlatformCreateDto(
    [property: Required] string Name,
    [property: Required] string Publisher,
    [property: Required] decimal Cost);

public sealed record PlatformPublishedDto(Guid Id, string Name, string Event = "");
