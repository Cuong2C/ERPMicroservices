using Microsoft.AspNetCore.Diagnostics;

namespace AuthService.Api.BootStraping;

public static class UseAuthServiceExtension
{
    public static WebApplication UseAuthServiceApi(this WebApplication app)
    {
        app.MapAuthServiceEndpoints();

        app.UseExceptionHandler(options =>
        {
            options.Run(async context =>
            {
                var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                if (exception != null)
                {
                    await exceptionHandler.TryHandleAsync(context, exception, context.RequestAborted);
                }
            });
        });

        return app;
    }
}
