using BuildingBlocks.Application.Interfaces;
using ItemCatalog.Api.Data.Seed;
using ItemCatalog.Api.Data.Seed.interfaces;
using ItemCatalog.Api.Services;
using Serilog;

namespace ItemCatalog.Api.BootStraping;

public static class ItemCatalogApplicationServiceExtension
{
    public static IServiceCollection AddItemCatalogApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        services.AddSerilog();

        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<ItemCatalogDbContext>(options =>
            options.UseNpgsql(connectionString));


        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDataSeeder, UnitSeeder>();
        services.AddScoped<IDataSeeder, TagSeeder>();
        services.AddScoped<IDataSeeder, CategorySeeder>();

        return services;
    }
}
