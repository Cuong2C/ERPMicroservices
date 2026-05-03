using AuthService.Api.Data.Seeds.Interfaces;

namespace AuthService.Api.BootStraping;

public class DatabaseInitializerExtension
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthServiceDbContext>();

        await context.Database.MigrateAsync();

        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}
