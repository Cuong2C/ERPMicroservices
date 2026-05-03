using AuthService.Api.Data.Seeds.Interfaces;

namespace AuthService.Api.Data.Seeds;

public class UserSeeder(AuthServiceDbContext context, IConfiguration configuration) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Users.AnyAsync()) return;

        var rootAdminPassword = configuration["RootAdmin:Password"]!;
        var rootAdminEmail = configuration["RootAdmin:Email"]!;

        var rootAdminUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "RootAdmin",
            PasswordHash = CustomHasher.HashByArgon2(rootAdminPassword),
            Fullname = "Root Administrator",
            Email = rootAdminEmail,
            Status = Status.Active,
            IsLocked = false
        };

        var rootAdminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == StaticDetail.ROLE_ROOT_ADMIN);

        if (rootAdminRole is null) return;

        rootAdminUser.UserRoles.Add(new UserRole
        {
            UserId = rootAdminUser.Id,
            RoleId = rootAdminRole.Id
        });


        context.Users.Add(rootAdminUser);
        await context.SaveChangesAsync();
    }
}
