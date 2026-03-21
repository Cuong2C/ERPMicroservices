using ItemCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemCatalog.Api.Data;

public class ItemCatalogDbContext : DbContext
{
    public ItemCatalogDbContext(DbContextOptions<ItemCatalogDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
