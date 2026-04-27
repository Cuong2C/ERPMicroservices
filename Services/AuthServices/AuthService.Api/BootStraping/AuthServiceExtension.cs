using AuthService.Api.Identity;
using AuthService.Api.Identity.Interfaces;
using AuthService.Api.Services.JwtServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        services.AddHttpContextAccessor();
        //services.AddScoped<IJwtService, JwtService>();

        // Configure JWT authentication
        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"] ?? "change_this_secret_in_production";
        var key = System.Text.Encoding.UTF8.GetBytes(secret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSection["Issuer"],
                ValidAudience = jwtSection["Audience"],
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key)
            };
        });

        services.AddScoped<ICurrentUserAuthService, CurrentUser>();
        services.AddScoped<ITenantGuard, TenantGuard>();
        services.AddScoped<IUserGuard, UserGuard>();
        services.AddScoped<IJwtService, JwtService>();


        return services;
    }
}
