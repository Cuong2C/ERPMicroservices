namespace ItemCatalog.Api.Data.Configurations;

public class UnitConfiguration : IEntityTypeConfiguration<MeasurementUnit>
{
    public void Configure(EntityTypeBuilder<MeasurementUnit> builder)
    {
        builder.HasKey(u => u.Id);
        builder.HasIndex(u => u.Code).IsUnique();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.HasIndex(u => u.Name).IsUnique();
        builder.HasMany(u => u.ItemUnits).WithOne(iu => iu.Unit).HasForeignKey(iu => iu.UnitId);
    }
}
