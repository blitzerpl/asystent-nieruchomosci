using AsystentNieruchomosci.Api;
using AsystentNieruchomosci.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

app.UseApiMiddleware();
app.MapApiEndpoints();

app.Run();
