namespace AuthService.Api.Data.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.HasKey(rc => new { rc.RoleId, rc.PermissionId });

        builder.HasOne(rc => rc.Role)
               .WithMany(r => r.RolePermissions)
               .HasForeignKey(rc => rc.RoleId);

        builder.HasOne(rc => rc.Permission)
               .WithMany()
               .HasForeignKey(rc => rc.PermissionId);
    }
}
