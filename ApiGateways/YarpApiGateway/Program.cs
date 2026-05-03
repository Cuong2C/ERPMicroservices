using Microsoft.IdentityModel.Tokens;
using YarpApiGateway.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var authConfig = builder.Configuration
    .GetSection("Authentication")
    .Get<JwtValidationOptions>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authConfig!.Authority;

        options.RequireHttpsMetadata = authConfig.RequireHttpsMetadata;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = authConfig.ValidateIssuer,
            ValidIssuer = authConfig.ValidIssuer,

            ValidateAudience = authConfig.ValidateAudience,
            ValidAudience = authConfig.ValidAudience,

            ValidateLifetime = authConfig.ValidateLifetime,

            ClockSkew = TimeSpan.FromSeconds(authConfig.ClockSkewSeconds)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
