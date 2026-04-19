namespace AuthService.Api.Data.Configurations;

public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
{
    public void Configure(EntityTypeBuilder<UserClaim> builder)
    {
        builder.HasKey(uc => new { uc.UserId, uc.ClaimId });

        builder.HasOne(uc => uc.User)
               .WithMany(u => u.UserClaims)
               .HasForeignKey(uc => uc.UserId);

        builder.HasOne(uc => uc.Claim)
               .WithMany()
               .HasForeignKey(uc => uc.ClaimId);
    }
}
