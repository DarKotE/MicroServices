using System.ComponentModel.DataAnnotations;

namespace CommandService.Models;

// public record Command(
//     [property: Required, Key] int Id,
//     [property: Required] string HowTo,
//     [property: Required] string CommandLine,
//     [property: Required] int PlatformId,
//     Platform Platform);
    
public record Command
{
    [Required, Key] public int Id { get; init; }
    [Required] public string HowTo { get; init; } = null!;
    [Required] public string CommandLine { get; init; } = null!;
    [Required] public int PlatformId { get; init; }
    public Platform? Platform { get; init; }
}