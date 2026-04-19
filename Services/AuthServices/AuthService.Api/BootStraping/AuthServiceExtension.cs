using AuthService.Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AuthService.Api.BootStraping;

public static class AuthServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        services.AddSerilog();

        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<AuthServiceDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
