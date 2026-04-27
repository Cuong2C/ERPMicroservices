namespace AuthService.Api.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
        builder.Property(u => u.Status).IsRequired();
        
        builder.HasMany(u => u.UserRoles).WithOne(ur => ur.User).HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.UserPermissions).WithOne(uc => uc.User).HasForeignKey(uc => uc.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(u => u.UserScopes).WithOne(us => us.User).HasForeignKey(us => us.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}
