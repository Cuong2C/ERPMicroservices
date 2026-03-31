using ItemCatalog.Api.Data.Seed.interfaces;
using ItemCatalog.Api.Items.CreateItem;
using ItemCatalog.Api.Items.GetItemById;

namespace ItemCatalog.Api.BootStraping;

public static class UseApplicationServiceExtension
{
    public static WebApplication UseItemCatalogApplicationServices(this WebApplication app)
    {
        app.MapCreateItemEndpoint();
        app.MapGetItemByIdEndpoint();

        return app;
    }
}
