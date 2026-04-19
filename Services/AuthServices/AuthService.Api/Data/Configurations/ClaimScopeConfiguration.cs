namespace AuthService.Api.Data.Configurations;

public class ClaimScopeConfiguration : IEntityTypeConfiguration<ClaimScope>
{
    public void Configure(EntityTypeBuilder<ClaimScope> builder)
    {
        builder.HasKey(cs => new { cs.ClaimId, cs.ScopeId });

        builder.HasOne(cs => cs.Claim)
               .WithMany(c => c.ClaimScopes)
               .HasForeignKey(cs => cs.ClaimId);

        builder.HasOne(cs => cs.Scope)
               .WithMany()
               .HasForeignKey(cs => cs.ScopeId);
    }
}
