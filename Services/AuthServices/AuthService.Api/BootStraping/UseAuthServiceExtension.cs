using AuthService.Api.Apis.Users.DeleteUser;
using AuthService.Api.Apis.Users.GetUserById;
using AuthService.Api.Apis.Users.GetUsers;
using AuthService.Api.Apis.Users.UpdateUser;
using AuthService.Api.Apis.Users.RegisterCustomerUser;
using AuthService.Api.Apis.Tenants.CreateTenant;
using AuthService.Api.Apis.Tenants.GetTenantById;
using AuthService.Api.Apis.Tenants.GetTenants;
using AuthService.Api.Apis.Tenants.UpdateTenant;
using AuthService.Api.Apis.Tenants.DeleteTenant;
using AuthService.Api.Apis.Users.CreateTenantUser;
using AuthService.Api.Apis.Users.RegisterTenantUser;
using AuthService.Api.Apis.Users.Login;
using AuthService.Api.Apis.Tokens.RefeshToken;
using AuthService.Api.Apis.OpenIdConnect.OpenIdConfiguration;
using AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;

namespace AuthService.Api.BootStraping;

public static class UseAuthServiceExtension
{
    public static IEndpointRouteBuilder UseAuthServiceApi(this IEndpointRouteBuilder app)
    {
        // OpenID Connect endpoints
        app.MapGetOpenIdConfigurationEndpoint();
        app.MapGetJsonWebKeySetEndpoint();

        // Tenent endpoints
        app.MapCreateTenantEndpoint();
        app.MapGetTenantsEndpoint();
        app.MapGetTenantByIdEndpoint();
        app.MapUpdateTenantEndpoint();
        app.MapDeleteTenantEndpoint();

        //  Token endpoints
        app.MapRefeshTokenEndpoint();

        // User endpoints
        app.MapCreateUserEndpoint();
        app.MapGetUsersEndpoint();
        app.MapGetUserByIdEndpoint();
        app.MapUpdateUserEndpoint();
        app.MapDeleteUserEndpoint();
        app.MapLoginEndpoint();
        app.MapRegisterTenantUserEndpoint();
        app.MapRegisterCustomerUserEndpoint();

        return app;
    }
}
