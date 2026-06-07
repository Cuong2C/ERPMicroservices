using AuthService.Api.Apis.OpenIdConnect.JsonWebKeySet;
using AuthService.Api.Apis.OpenIdConnect.OpenIdConfiguration;
using AuthService.Api.Apis.Tenants.CreateTenant;
using AuthService.Api.Apis.Tenants.DeleteTenant;
using AuthService.Api.Apis.Tenants.GetTenantById;
using AuthService.Api.Apis.Tenants.GetTenants;
using AuthService.Api.Apis.Tenants.UpdateTenant;
using AuthService.Api.Apis.Tokens.IntrospectToken;
using AuthService.Api.Apis.Tokens.RefeshToken;
using AuthService.Api.Apis.Users.DeleteUser;
using AuthService.Api.Apis.Users.GetUserById;
using AuthService.Api.Apis.Users.GetUsers;
using AuthService.Api.Apis.Users.Login;
using AuthService.Api.Apis.Users.RegisterCustomerUser;
using AuthService.Api.Apis.Users.UpdateUser;

namespace AuthService.Api.BootStraping;

public static class AuthServcieEndpointsExtension
{
    public static IEndpointRouteBuilder MapAuthServiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // OpenID Connect endpoints
        endpoints.MapGetOpenIdConfigurationEndpoint();
        endpoints.MapGetJsonWebKeySetEndpoint();

        // Tenent endpoints
        endpoints.MapCreateTenantEndpoint();
        endpoints.MapGetTenantsEndpoint();
        endpoints.MapGetTenantByIdEndpoint();
        endpoints.MapUpdateTenantEndpoint();
        endpoints.MapDeleteTenantEndpoint();

        //  Token endpoints
        endpoints.MapIntrospectTokenEndpoint();
        endpoints.MapRefeshTokenEndpoint();

        // User endpoints
        endpoints.MapGetUsersEndpoint();
        endpoints.MapGetUserByIdEndpoint();
        endpoints.MapUpdateUserEndpoint();
        endpoints.MapDeleteUserEndpoint();
        endpoints.MapLoginEndpoint();
        endpoints.MapRegisterCustomerUserEndpoint();

        return endpoints;
    }
}
