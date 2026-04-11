using ItemCatalog.Api.Items.CreateItem;
using ItemCatalog.Api.Items.DeleteItem;
using ItemCatalog.Api.Items.GetDeletedItems;
using ItemCatalog.Api.Items.GetItemById;
using ItemCatalog.Api.Items.GetItems;
using ItemCatalog.Api.Items.UpdateItem;
using ItemCatalog.Api.Units.CreateUnit;
using ItemCatalog.Api.Units.GetUnits;
using ItemCatalog.Api.Units.GetUnitById;
using ItemCatalog.Api.Units.UpdateUnit;
using ItemCatalog.Api.Units.DeleteUnit;
using ItemCatalog.Api.Categories.CreateCategory;
using ItemCatalog.Api.Categories.GetCategories;
using ItemCatalog.Api.Categories.GetCategoryById;
using ItemCatalog.Api.Categories.UpdateCategory;
using ItemCatalog.Api.Categories.DeleteCategory;
using ItemCatalog.Api.Tags.CreateTag;
using ItemCatalog.Api.Tags.GetTags;
using ItemCatalog.Api.Tags.GetTagById;
using ItemCatalog.Api.Tags.UpdateTag;
using ItemCatalog.Api.Tags.DeleteTag;
using Microsoft.AspNetCore.Diagnostics;

namespace ItemCatalog.Api.BootStraping;

public static class UseApplicationServiceExtension
{
    public static WebApplication UseItemCatalogApplicationServices(this WebApplication app)
    {
        app.MapCreateItemEndpoint();
        app.MapGetItemByIdEndpoint();
        app.MapCreateUnitEndpoint();
        app.MapGetUnitsEndpoint();
        app.MapGetUnitByIdEndpoint();
        app.MapUpdateUnitEndpoint();
        app.MapDeleteUnitEndpoint();
        app.MapCreateCategoryEndpoint();
        app.MapGetCategoriesEndpoint();
        app.MapGetCategoryByIdEndpoint();
        app.MapUpdateCategoryEndpoint();
        app.MapDeleteCategoryEndpoint();
        app.MapCreateTagEndpoint();
        app.MapGetTagsEndpoint();
        app.MapGetTagByIdEndpoint();
        app.MapUpdateTagEndpoint();
        app.MapDeleteTagEndpoint();
        app.MapGetItemsEndpoint();
        app.MapUpdateItemEndpoint();
        app.MapDeleteItemEndpoint();
        app.MapGetDeletedItemsEndpoint();

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
