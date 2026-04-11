namespace ItemCatalog.Api.Data.Configurations;

public class ItemSellPriceConfiguration
{
    public void Configure(EntityTypeBuilder<ItemSellPrice> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasIndex(i => new { i.TenantId, i.ItemId, i.EffectiveDate }).IsUnique();
        builder.Property(p => p.Price).HasColumnType("decimal(18,4)").IsRequired();
        builder.Property(p => p.EffectiveDate).IsRequired();
    }
    
}
