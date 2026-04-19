namespace AuthService.Api.Data.Configurations;

public class ClaimValueConfiguration : IEntityTypeConfiguration<ClaimValue>
{
    public void Configure(EntityTypeBuilder<ClaimValue> builder)
    {
        builder.HasKey(cv => cv.Id);
        builder.Property(cv => cv.Name).IsRequired().HasMaxLength(255);
    }
}
