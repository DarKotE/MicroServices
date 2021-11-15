using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using PlatformService.Models;

namespace PlatformService.Data;

public static class TestDb
{
    public static void Populate(IApplicationBuilder app, bool isProduction)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        var context = scope.ServiceProvider.GetService<AppDbContext>();
        ArgumentNullException.ThrowIfNull(context);
        
        SeedData(context, isProduction);
        
        static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
            
            if (!context.Platforms.Any())
            {
                Console.WriteLine("Seeding...");
                context.Platforms.AddRange(
                    new Platform (Guid.NewGuid(), "Name1", "Publisher1", 0),
                    new Platform (Guid.NewGuid(), "Name2", "Publisher2", 1),
                    new Platform (Guid.NewGuid(), "Name3", "Publisher3", 3),
                    new Platform (Guid.NewGuid(), "Name4", "Publisher4", 5));
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("Failed to populate context. Already has data!");
            }
        }
    }
}