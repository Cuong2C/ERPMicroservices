using AuthService.Api.Data.Seeds.Interfaces;

namespace AuthService.Api.Data.Seeds;

public class PermissionActionSeeder(AuthServiceDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.PermissionActions.AnyAsync()) return;

        context.PermissionActions.AddRange(
            new PermissionAction { Name = "All" },
            new PermissionAction { Name = "Create" },
            new PermissionAction { Name = "Read" },
            new PermissionAction { Name = "Update" },
            new PermissionAction { Name = "Delete" },
            new PermissionAction { Name = "Approve" },
            new PermissionAction { Name = "Publish" },
            new PermissionAction { Name = "Print" },
            new PermissionAction { Name = "Export" },
            new PermissionAction { Name = "Import" }
        );

        await context.SaveChangesAsync();
    }
}
