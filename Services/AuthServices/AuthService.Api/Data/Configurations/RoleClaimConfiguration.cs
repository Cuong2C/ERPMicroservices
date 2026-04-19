namespace AuthService.Api.Data.Configurations;

public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.HasKey(rc => new { rc.RoleId, rc.ClaimId });

        builder.HasOne(rc => rc.Role)
               .WithMany(r => r.RoleClaims)
               .HasForeignKey(rc => rc.RoleId);

        builder.HasOne(rc => rc.Claim)
               .WithMany()
               .HasForeignKey(rc => rc.ClaimId);
    }
}
