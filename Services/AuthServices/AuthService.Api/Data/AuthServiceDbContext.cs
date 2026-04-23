using System.Reflection;

namespace AuthService.Api.Data;

public class AuthServiceDbContext : DbContext
{
    public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<Resource> Resources { get; set; }
    public DbSet<Scope> Scopes { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserClaim> UserClaims { get; set; }
    public DbSet<UserScope> UserScopes { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
    public DbSet<ClaimScope> ClaimScopes { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
