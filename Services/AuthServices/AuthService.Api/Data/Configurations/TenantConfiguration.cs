namespace AuthService.Api.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.HasIndex(t => t.Name).IsUnique();

        builder.Property(t => t.Description).HasMaxLength(1000).IsRequired(false);
        builder.Property(t => t.Address).HasMaxLength(500).IsRequired(false);
        builder.Property(t => t.City).HasMaxLength(255).IsRequired(false);
        builder.Property(t => t.Region).HasMaxLength(255).IsRequired(false);
        builder.Property(t => t.PostalCode).IsRequired(false);
        builder.Property(t => t.Country).HasMaxLength(255).IsRequired(false);
        builder.Property(t => t.PhoneNumber).HasMaxLength(50).IsRequired(false);
    }
}
