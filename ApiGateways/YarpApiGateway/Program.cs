using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using YarpApiGateway.Options;
using YarpApiGateway.StaticDetails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var authConfig = builder.Configuration
    .GetSection("JwtOptions")
    .Get<JwtValidationOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var jti = context.Principal?.FindFirst("jti")?.Value;

                var httpClient = context.HttpContext.RequestServices
                    .GetRequiredService<HttpClient>();

                var response = await httpClient.GetAsync(
                    $"{authConfig!.Authority}/tokens/introspect?jti={jti}"
                );

                if (!response.IsSuccessStatusCode)
                {
                    context.Fail("Token invalid");
                }
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    options.AddPolicy(StaticDetail.ROLE_SHOP_OWNER, policy =>
    {
        policy.RequireRole(StaticDetail.ROLE_SHOP_OWNER, StaticDetail.ROLE_ROOT_ADMIN);
    });

    options.AddPolicy(StaticDetail.ROLE_CUSTOMER, policy =>
    {
        policy.RequireRole(StaticDetail.ROLE_CUSTOMER, StaticDetail.ROLE_ROOT_ADMIN);
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

app.Run();
