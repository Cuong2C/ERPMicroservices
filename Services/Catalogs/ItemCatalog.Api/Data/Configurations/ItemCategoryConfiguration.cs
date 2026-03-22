namespace ItemCatalog.Api.Data.Configurations;

public class ItemCategoryConfiguration : IEntityTypeConfiguration<ItemCategory>
{
    public void Configure(EntityTypeBuilder<ItemCategory> builder)
    {
        builder.HasKey(ic => new { ic.ItemId, ic.CategoryId });
    }
}
