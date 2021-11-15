using CommandService.DataServices.Sync.Grpc;
using CommandService.Models;

namespace CommandService.Data;

public static class TestDb
{
    public static void Populate(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var grpcClient = scope.ServiceProvider.GetService<IPlatformDataClient>();
        ArgumentNullException.ThrowIfNull(grpcClient);
        var platforms = grpcClient.ReturnAllPlatforms();
        var repo = scope.ServiceProvider.GetService<ICommandRepo>();
        ArgumentNullException.ThrowIfNull(repo);
        SeedData(repo, platforms);
        
        static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            try
            {
                foreach (var platform in platforms)
                {
                    if (!repo.Exists(platform.OriginId))
                        repo.CreatePlatform(platform);
                }

                repo.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}