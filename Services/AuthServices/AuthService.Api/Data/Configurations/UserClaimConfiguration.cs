namespace AuthService.Api.Data.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.HasKey(uc => new { uc.UserId, uc.PermissionId });

        builder.HasOne(uc => uc.User)
               .WithMany(u => u.UserPermissions)
               .HasForeignKey(uc => uc.UserId);

        builder.HasOne(uc => uc.Permission)
               .WithMany()
               .HasForeignKey(uc => uc.PermissionId);
    }
}
