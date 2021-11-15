using CommandService.Models;

namespace CommandService.Data;

public sealed class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _context;

    public CommandRepo(AppDbContext context) => _context = context;

    public bool SaveChanges() => _context.SaveChanges() >= 0;

    public IEnumerable<Platform> GetAllPlatforms() => _context.Platforms.ToList();

    public IEnumerable<Command> GetCommandsForPlatform(int platformId) => _context.Commands.Where(p=>p.PlatformId == platformId);

    public Command? GetCommand(int platformId, int commandId) => _context.Commands.FirstOrDefault(p => p.PlatformId == platformId && p.Id == commandId);

    public void CreatePlatform(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        _context.Platforms.Add(platform);
    }

    public void CreateCommand(int platformId, Command command)
    {
        ArgumentNullException.ThrowIfNull(command);
        _context.Commands.Add(command with { PlatformId = platformId });
    }

    public bool Exists(int platformId) => _context.Platforms.Any(p=>p.Id == platformId);
    public bool Exists(Guid originalPlatformId) => _context.Platforms.Any(p=>p.OriginId == originalPlatformId);
}