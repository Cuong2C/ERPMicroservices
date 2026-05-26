using AuthService.Api.BootStraping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    await DatabaseInitializerExtension.InitializeAsync(app);
}

app.UseAuthServiceApi();

app.Run();
