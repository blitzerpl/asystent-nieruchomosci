using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace AsystentNieruchomosci.Api.IntegrationTests;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["CorsSettings:AllowedOrigins:0"] = "http://localhost:4200",
                ["CorsSettings:AllowedOrigins:1"] = "https://localhost:4200",
                ["CorsSettings:AllowedMethods:0"] = "GET",
                ["CorsSettings:AllowedMethods:1"] = "POST",
                ["CorsSettings:AllowedMethods:2"] = "PUT",
                ["CorsSettings:AllowedMethods:3"] = "DELETE",
                ["CorsSettings:AllowedMethods:4"] = "OPTIONS",
                ["CorsSettings:AllowedHeaders:0"] = "Content-Type",
                ["CorsSettings:AllowedHeaders:1"] = "Authorization",
                ["CorsSettings:AllowedHeaders:2"] = "X-Correlation-Id",
                ["CorsSettings:AllowCredentials"] = "true"
            });
        });

        builder.UseEnvironment("Development");
    }
}
