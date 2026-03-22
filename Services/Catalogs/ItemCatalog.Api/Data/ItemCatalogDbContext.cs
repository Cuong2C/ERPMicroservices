using ItemCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ItemCatalog.Api.Data;

public class ItemCatalogDbContext : DbContext
{
    public ItemCatalogDbContext(DbContextOptions<ItemCatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<MeasurementUnit> Units { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<ItemUnit> ItemUnits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

}
