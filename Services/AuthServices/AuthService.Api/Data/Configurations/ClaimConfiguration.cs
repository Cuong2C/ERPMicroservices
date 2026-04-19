namespace AuthService.Api.Data.Configurations;

public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
{
    public void Configure(EntityTypeBuilder<Claim> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Type).IsRequired().HasMaxLength(255);

        builder.HasOne(c => c.Value)
               .WithMany()
               .HasForeignKey(c => c.ClaimValueId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.ClaimScopes)
               .WithOne(cs => cs.Claim)
               .HasForeignKey(cs => cs.ClaimId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
