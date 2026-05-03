using AuthService.Api.Data.Seeds.Interfaces;

namespace AuthService.Api.Data.Seeds;

public class ResourceSeeder(AuthServiceDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Resources.AnyAsync()) return;

        context.Resources.AddRange(
            new Resource { Name = "Warehouse" },
            new Resource { Name = "Category" },
            new Resource { Name = "Tax" }
        );

        await context.SaveChangesAsync();
    }
}
