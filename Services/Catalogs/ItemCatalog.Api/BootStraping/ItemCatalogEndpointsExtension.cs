using ItemCatalog.Api.Apis.Categories.CreateCategory;
using ItemCatalog.Api.Apis.Categories.DeleteCategory;
using ItemCatalog.Api.Apis.Categories.GetCategories;
using ItemCatalog.Api.Apis.Categories.GetCategoryById;
using ItemCatalog.Api.Apis.Categories.UpdateCategory;
using ItemCatalog.Api.Apis.Items.CreateItem;
using ItemCatalog.Api.Apis.Items.DeleteItem;
using ItemCatalog.Api.Apis.Items.GetDeletedItems;
using ItemCatalog.Api.Apis.Items.GetItemById;
using ItemCatalog.Api.Apis.Items.GetItems;
using ItemCatalog.Api.Apis.Items.UpdateItem;
using ItemCatalog.Api.Apis.ItemSellPrices.CreateItemSellPrice;
using ItemCatalog.Api.Apis.ItemSellPrices.DeleteItemSellPrice;
using ItemCatalog.Api.Apis.ItemSellPrices.GetItemSellPriceById;
using ItemCatalog.Api.Apis.ItemSellPrices.GetItemSellPricesByItemId;
using ItemCatalog.Api.Apis.ItemSellPrices.GetLatestItemSellPrice;
using ItemCatalog.Api.Apis.ItemSellPrices.UpdateItemSellPrice;
using ItemCatalog.Api.Apis.Tags.CreateTag;
using ItemCatalog.Api.Apis.Tags.DeleteTag;
using ItemCatalog.Api.Apis.Tags.GetTagById;
using ItemCatalog.Api.Apis.Tags.GetTags;
using ItemCatalog.Api.Apis.Tags.UpdateTag;
using ItemCatalog.Api.Apis.Units.CreateUnit;
using ItemCatalog.Api.Apis.Units.DeleteUnit;
using ItemCatalog.Api.Apis.Units.GetUnitById;
using ItemCatalog.Api.Apis.Units.GetUnits;
using ItemCatalog.Api.Apis.Units.UpdateUnit;

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
