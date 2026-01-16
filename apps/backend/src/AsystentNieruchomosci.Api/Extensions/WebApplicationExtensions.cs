using AsystentNieruchomosci.Api.Endpoints;
using AsystentNieruchomosci.Api.Middleware;

namespace AsystentNieruchomosci.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseApiMiddleware(this WebApplication app)
    {
        app.UseMiddleware<CorrelationIdMiddleware>();
        app.UseExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }

    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        var rootGroup = app.MapGroup(string.Empty);
        rootGroup.MapHealthEndpoints();

        return app;
    }
}

