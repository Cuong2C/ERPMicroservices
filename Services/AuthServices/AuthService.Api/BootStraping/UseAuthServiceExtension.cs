using AuthService.Api.Apis.Users.CreateUser;
using AuthService.Api.Apis.Users.DeleteUser;
using AuthService.Api.Apis.Users.GetUserById;
using AuthService.Api.Apis.Users.GetUsers;
using AuthService.Api.Apis.Users.UpdateUser;

namespace AuthService.Api.BootStraping;

public static class UseAuthServiceExtension
{
    public static WebApplication UseAuthServiceApi(this WebApplication app)
    {
        app.MapCreateUserEndpoint();
        app.MapGetUsersEndpoint();
        app.MapGetUserByIdEndpoint();
        app.MapUpdateUserEndpoint();
        app.MapDeleteUserEndpoint();

        return app;
    }
}
