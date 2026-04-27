namespace AuthService.Api.Data.Configurations;

public class ClaimValueConfiguration : IEntityTypeConfiguration<PermissionAction>
{
    public void Configure(EntityTypeBuilder<PermissionAction> builder)
    {
        builder.HasKey(cv => cv.Id);
        builder.Property(cv => cv.Name).IsRequired().HasMaxLength(255);
    }
}
