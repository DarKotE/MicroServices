using System.ComponentModel.DataAnnotations;

namespace CommandService.Dto;

public record CommandReadDto(int Id, string HowTo, string CommandLine, int PlatformId);
public record CommandCreateDto
{
    [Required] public string HowTo { get; init; } = null!;
    [Required] public string CommandLine { get; init; } = null!;
}