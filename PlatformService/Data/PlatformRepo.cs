using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;

    public PlatformRepo(AppDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        _context = context;
    }

    public bool SaveChanges() => _context.SaveChanges() >= 0;

    public IEnumerable<Platform?> GetAll() => _context.Platforms.ToList().AsReadOnly();

    public Platform? GetById(Guid id) => _context.Platforms.FirstOrDefault(p => p.Id == id);

    public void Create(Platform platform)
    {
        ArgumentNullException.ThrowIfNull(platform);
        _context.Platforms.Add(platform);
    }
}