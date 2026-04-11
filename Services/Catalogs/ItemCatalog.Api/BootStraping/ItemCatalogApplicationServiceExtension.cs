using BuildingBlocks.Application.Interfaces;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Infrastructure;
using ItemCatalog.Api.Data.Seed;
using ItemCatalog.Api.Data.Seed.interfaces;
using ItemCatalog.Api.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        services.AddDbContext<ItemCatalogDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITenantGuard, TenantGuard>();
        services.AddScoped<IDataSeeder, UnitSeeder>();
        services.AddScoped<IDataSeeder, TagSeeder>();
        services.AddScoped<IDataSeeder, CategorySeeder>();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
