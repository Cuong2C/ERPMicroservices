using ItemCatalog.Api.Categories.CreateCategory;
using ItemCatalog.Api.Categories.DeleteCategory;
using ItemCatalog.Api.Categories.GetCategories;
using ItemCatalog.Api.Categories.GetCategoryById;
using ItemCatalog.Api.Categories.UpdateCategory;
using ItemCatalog.Api.Items.CreateItem;
using ItemCatalog.Api.Items.DeleteItem;
using ItemCatalog.Api.Items.GetDeletedItems;
using ItemCatalog.Api.Items.GetItemById;
using ItemCatalog.Api.Items.GetItems;
using ItemCatalog.Api.Items.UpdateItem;
using ItemCatalog.Api.ItemSellPrices.CreateItemSellPrice;
using ItemCatalog.Api.ItemSellPrices.DeleteItemSellPrice;
using ItemCatalog.Api.ItemSellPrices.GetItemSellPriceById;
using ItemCatalog.Api.ItemSellPrices.GetItemSellPrices;
using ItemCatalog.Api.ItemSellPrices.GetLatestItemSellPrice;
using ItemCatalog.Api.ItemSellPrices.UpdateItemSellPrice;
using ItemCatalog.Api.Tags.CreateTag;
using ItemCatalog.Api.Tags.DeleteTag;
using ItemCatalog.Api.Tags.GetTagById;
using ItemCatalog.Api.Tags.GetTags;
using ItemCatalog.Api.Tags.UpdateTag;
using ItemCatalog.Api.Units.CreateUnit;
using ItemCatalog.Api.Units.DeleteUnit;
using ItemCatalog.Api.Units.GetUnitById;
using ItemCatalog.Api.Units.GetUnits;
using ItemCatalog.Api.Units.UpdateUnit;

namespace ItemCatalog.Api.BootStraping;

public static class ItemCatalogEndpointsExtension
{
    public static IEndpointRouteBuilder MapItemCatalogEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapCreateItemEndpoint();
        endpoints.MapGetItemByIdEndpoint();
        endpoints.MapCreateUnitEndpoint();
        endpoints.MapGetUnitsEndpoint();
        endpoints.MapGetUnitByIdEndpoint();
        endpoints.MapUpdateUnitEndpoint();
        endpoints.MapDeleteUnitEndpoint();
        endpoints.MapCreateCategoryEndpoint();
        endpoints.MapGetCategoriesEndpoint();
        endpoints.MapGetCategoryByIdEndpoint();
        endpoints.MapUpdateCategoryEndpoint();
        endpoints.MapDeleteCategoryEndpoint();
        endpoints.MapCreateTagEndpoint();
        endpoints.MapGetTagsEndpoint();
        endpoints.MapGetTagByIdEndpoint();
        endpoints.MapUpdateTagEndpoint();
        endpoints.MapDeleteTagEndpoint();
        endpoints.MapGetItemsEndpoint();
        endpoints.MapUpdateItemEndpoint();
        endpoints.MapDeleteItemEndpoint();
        endpoints.MapGetDeletedItemsEndpoint();

        endpoints.MapCreateItemSellPriceEndpoint();
        endpoints.MapDeleteItemSellPriceEndpoint();
        endpoints.MapUpdateItemSellPriceEndpoint();
        endpoints.MapGetItemSellPriceEndpoint();
        endpoints.MapGetItemSellPricesEndpoint();
        endpoints.MapGetLatestItemSellPriceEndpoint();

        return endpoints;
    }
}
