using System.ComponentModel.DataAnnotations;

namespace CommandService.Models;

public record Platform
{
    [Required, Key] public int Id { get; init; }
    [Required] public Guid OriginId { get; init; }
    [Required] public string Name { get; init; }
    public ICollection<Command> Commands { get; init; }
}