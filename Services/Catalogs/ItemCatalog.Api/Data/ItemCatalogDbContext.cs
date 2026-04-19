using System.Reflection;

namespace ItemCatalog.Api.Data;

public class ItemCatalogDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;
    public ItemCatalogDbContext(DbContextOptions<ItemCatalogDbContext> options, ICurrentUser currentUser) : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<MeasurementUnit> Units { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<ItemUnit> ItemUnits { get; set; }
    public DbSet<ItemSellPrice> ItemSellPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Item>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId);

        // Allow global datas(TenantId == Guid.Empty), which are shared across tenants
        modelBuilder.Entity<MeasurementUnit>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId || x.TenantId == Guid.Empty);

        modelBuilder.Entity<ItemUnit>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId);

        modelBuilder.Entity<Category>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId || x.TenantId == Guid.Empty);

        modelBuilder.Entity<ItemCategory>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId);

        modelBuilder.Entity<Tag>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId || x.TenantId == Guid.Empty);

        modelBuilder.Entity<ItemSellPrice>()
                .HasQueryFilter(x => x.TenantId == _currentUser.TenantId);

        base.OnModelCreating(modelBuilder);
    }

}
