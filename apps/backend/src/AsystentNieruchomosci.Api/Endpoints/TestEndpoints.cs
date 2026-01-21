using AsystentNieruchomosci.Application.Common.Exceptions;

namespace AsystentNieruchomosci.Api.Endpoints;

public static class TestEndpoints
{
    public static RouteGroupBuilder MapTestEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/test/validation", () =>
        {
            throw new ValidationException("Test validation error");
        });

        group.MapGet("/test/not-found", () =>
        {
            throw new NotFoundException("Test resource not found");
        });

        group.MapGet("/test/argument-null", () =>
        {
            throw new ArgumentNullException("param", "Test argument null error");
        });

        group.MapGet("/test/argument", () =>
        {
            throw new ArgumentException("Test argument error", "param");
        });

        group.MapGet("/test/unauthorized", () =>
        {
            throw new UnauthorizedAccessException("Test unauthorized error");
        });

        group.MapGet("/test/server-error", () =>
        {
            throw new Exception("Test server error");
        });

        group.MapGet("/test/inner-exception", () =>
        {
            throw new Exception("Test outer exception", new InvalidOperationException("Test inner exception"));
        });

        return group;
    }
}
