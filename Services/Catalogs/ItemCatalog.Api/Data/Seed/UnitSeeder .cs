using ItemCatalog.Api.Data.Seed.interfaces;

namespace ItemCatalog.Api.Data.Seed;

public class UnitSeeder(ItemCatalogDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Units.AnyAsync()) return;

        context.Units.AddRange(
            new MeasurementUnit { Code = "Piece", Name = "Piece" },
            new MeasurementUnit { Code = "Kg", Name = "Kilogram" },
            new MeasurementUnit { Code = "Gram", Name = "Gram" },
            new MeasurementUnit { Code = "Lit", Name = "Liter" },
            new MeasurementUnit { Code = "Ml", Name = "Milliliter" },
            new MeasurementUnit { Code = "Package", Name = "Package" }
        );

        await context.SaveChangesAsync();
    }
}

