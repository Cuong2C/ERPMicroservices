using ItemCatalog.Api.Data;
using Microsoft.EntityFrameworkCore;
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

        return services;
    }
}
