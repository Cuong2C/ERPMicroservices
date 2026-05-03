using AuthService.Api.Data.Seeds.Interfaces;

namespace AuthService.Api.Data.Seeds;

public class RoleSeeder(AuthServiceDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Roles.AnyAsync()) return;

        context.Roles.AddRange(
            new Role { Name = StaticDetail.ROLE_ROOT_ADMIN, Description = "Root administrator with full access" },
            new Role { Name = StaticDetail.ROLE_SHOP_OWNER, Description = "Shop owner with limited access by tenant" },
            new Role { Name = StaticDetail.ROLE_CUSTOMER, Description = "Customer with basic access" }
        );

        await context.SaveChangesAsync();
    }
}
