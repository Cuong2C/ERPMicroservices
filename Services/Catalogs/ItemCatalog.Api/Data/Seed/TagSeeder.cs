using ItemCatalog.Api.Data.Seed.interfaces;

namespace ItemCatalog.Api.Data.Seed;

public class TagSeeder(ItemCatalogDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Tags.AnyAsync()) return;

        context.Tags.AddRange(
            new Tag { Code = "Fragile", Name = "Fragile" },
            new Tag { Code = "Perishable", Name = "Perishable" },
            new Tag { Code = "New", Name = "New" },
            new Tag { Code = "Sale", Name = "Sale" },
            new Tag { Code = "Imported", Name = "Imported" }
        );

        await context.SaveChangesAsync();
    }
}
