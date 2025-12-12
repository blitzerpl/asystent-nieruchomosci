using AsystentNieruchomosci.Application.Health.Queries.GetHealth;
using MediatR;

namespace AsystentNieruchomosci.Api.Endpoints;

public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/health", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetHealthQuery());
            return Results.Ok(result);
        })
        .WithName("GetHealth")
        .WithSummary("Health check endpoint")
        .WithDescription("Returns the health status of the API including environment and correlation ID");

        return group;
    }
}

