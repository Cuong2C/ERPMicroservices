namespace ItemCatalog.Api.Data.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);
        builder.HasIndex(i => new { i.TenantId, i.Code }).IsUnique();
        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
    }
}
