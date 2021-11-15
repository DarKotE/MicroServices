using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepo
{
    bool SaveChanges();
    IEnumerable<Platform?> GetAll();
    Platform? GetById(Guid id);
    void Create(Platform platform);
}