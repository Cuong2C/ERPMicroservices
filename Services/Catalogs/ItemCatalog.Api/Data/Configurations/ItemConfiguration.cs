namespace ItemCatalog.Api.Data.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name).IsRequired().HasMaxLength(200);
        builder.Property(i => i.BaseUnitId).IsRequired();
        builder.Property(i => i.Description).HasMaxLength(1000);
        builder.Property(i => i.ImageUrl).HasMaxLength(500);
        builder.HasMany(i => i.ItemCategories).WithOne(ic => ic.Item).HasForeignKey(ic => ic.ItemId);
        builder.HasMany(i => i.ItemUnits).WithOne(iu => iu.Item).HasForeignKey(iu => iu.ItemId);
        builder.HasMany(i => i.Tags).WithMany();
    }
}
