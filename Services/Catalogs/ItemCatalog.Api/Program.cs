using ItemCatalog.Api.BootStraping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddItemCatalogApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseItemCatalogApplicationServices();

app.Run();
