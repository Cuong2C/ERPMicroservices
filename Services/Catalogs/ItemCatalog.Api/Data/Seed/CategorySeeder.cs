using ItemCatalog.Api.Data.Seed.interfaces;

namespace ItemCatalog.Api.Data.Seed;

public class CategorySeeder(ItemCatalogDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (await context.Categories.AnyAsync()) return;

        context.Categories.AddRange(
            new Category { Code = "ELEC", Name = "Electronics" },
            new Category { Code = "FOOD", Name = "Food" },
            new Category { Code = "HOME", Name = "Home & Living" },
            new Category { Code = "CLOTH", Name = "Clothing" },
            new Category { Code = "BEAUTY", Name = "Beauty" }
        );

        await context.SaveChangesAsync();
    }
}
