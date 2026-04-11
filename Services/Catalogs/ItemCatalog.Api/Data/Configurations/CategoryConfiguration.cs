namespace ItemCatalog.Api.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasIndex(i => new { i.TenantId, i.Code }).IsUnique();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.HasMany(c => c.ItemCategories).WithOne(ic => ic.Category).HasForeignKey(ic => ic.CategoryId);
    }
}
