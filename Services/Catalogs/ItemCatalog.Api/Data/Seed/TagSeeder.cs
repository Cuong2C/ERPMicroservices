using ItemCatalog.Api.Data.Seed.interfaces;

namespace ItemCatalog.Api.Data.Seed;

public class TagSeeder(ItemCatalogDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Tags.AnyAsync()) return;

        context.Tags.AddRange(
            new Tag { Name = "Fragile" },
            new Tag { Name = "Perishable" },
            new Tag { Name = "New" },
            new Tag { Name = "Sale" },
            new Tag { Name = "Imported" }
        );

        await context.SaveChangesAsync();
    }
}
