namespace CommandService.Dto;

public record PlatformReadDto(int Id, string Name);
public record PlatformPublishedDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Event { get; init; }
}
