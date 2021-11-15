using CommandService.Models;

namespace CommandService.Data;

public interface ICommandRepo
{
    bool SaveChanges();
    IEnumerable<Platform> GetAllPlatforms();
    IEnumerable<Command> GetCommandsForPlatform(int platformId);
    Command? GetCommand(int platformId, int commandId);
    void CreatePlatform(Platform platform);
    void CreateCommand(int platformId, Command command);
    bool Exists(int platformId);
    bool Exists(Guid originalPlatformId);
}