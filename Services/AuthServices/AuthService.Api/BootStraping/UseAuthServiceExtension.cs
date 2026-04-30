using AuthService.Api.Apis.Users.CreateUser;
using AuthService.Api.Apis.Users.DeleteUser;
using AuthService.Api.Apis.Users.GetUserById;
using AuthService.Api.Apis.Users.GetUsers;
using AuthService.Api.Apis.Users.UpdateUser;
using AuthService.Api.Apis.Tenants.CreateTenant;
using AuthService.Api.Apis.Tenants.GetTenantById;
using AuthService.Api.Apis.Tenants.GetTenants;
using AuthService.Api.Apis.Tenants.UpdateTenant;
using AuthService.Api.Apis.Tenants.DeleteTenant;

namespace AuthService.Api.BootStraping;

public static class UseAuthServiceExtension
{
    public static WebApplication UseAuthServiceApi(this WebApplication app)
    {
        app.MapCreateUserEndpoint();
        app.MapCreateTenantEndpoint();
        app.MapGetTenantsEndpoint();
        app.MapGetTenantByIdEndpoint();
        app.MapUpdateTenantEndpoint();
        app.MapDeleteTenantEndpoint();
        app.MapGetUsersEndpoint();
        app.MapGetUserByIdEndpoint();
        app.MapUpdateUserEndpoint();
        app.MapDeleteUserEndpoint();

        return app;
    }
}
