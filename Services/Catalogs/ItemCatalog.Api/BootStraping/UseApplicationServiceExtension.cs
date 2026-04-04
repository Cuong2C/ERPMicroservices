using ItemCatalog.Api.Items.CreateItem;
using ItemCatalog.Api.Items.GetItemById;
using Microsoft.AspNetCore.Diagnostics;

namespace ItemCatalog.Api.BootStraping;

public static class UseApplicationServiceExtension
{
    public static WebApplication UseItemCatalogApplicationServices(this WebApplication app)
    {
        app.MapCreateItemEndpoint();
        app.MapGetItemByIdEndpoint();

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
