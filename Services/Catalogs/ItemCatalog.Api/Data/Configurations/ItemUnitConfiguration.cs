namespace ItemCatalog.Api.Data.Configurations;

public class ItemUnitConfiguration : IEntityTypeConfiguration<ItemUnit>
{
    public void Configure(EntityTypeBuilder<ItemUnit> builder)
    {
        builder.HasKey(iu => new { iu.ItemId, iu.UnitId });
    }
}
