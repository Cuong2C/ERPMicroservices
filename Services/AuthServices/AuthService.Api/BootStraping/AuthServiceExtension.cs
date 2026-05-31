using AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;
using AuthService.Api.Data.Seeds;
using AuthService.Api.Data.Seeds.Interfaces;
using AuthService.Api.Services.JwtServices.Interfaces;
using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
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

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<AuthServiceDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetRequiredService<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITenantGuard, TenantGuard>();
        services.AddScoped<IUserGuard, UserGuard>();
        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IDataSeeder, PermissionActionSeeder>();
        services.AddScoped<IDataSeeder, ResourceSeeder>();
        services.AddScoped<IDataSeeder, RoleSeeder>();
        services.AddScoped<IDataSeeder, UserSeeder>();


        services.AddSingleton<RsaKeyProvider>();
        services.AddSingleton<JwksProvider>();
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddExceptionHandler<GlobalExceptionHandler>();


        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = configuration.GetSection("JwtOptions").GetValue<string>("Authority");

                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("JwtOptions").GetValue<string>("ValidIssuer"),

                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("JwtOptions").GetValue<string>("ValidAudience"),   
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.FromSeconds(5)
                };
            });

        return services;
    }
}
