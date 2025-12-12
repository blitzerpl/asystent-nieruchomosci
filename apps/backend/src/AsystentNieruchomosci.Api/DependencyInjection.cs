using AsystentNieruchomosci.Api.Services;
using AsystentNieruchomosci.Application;
using AsystentNieruchomosci.Application.Common.Interfaces;

namespace AsystentNieruchomosci.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddApplication();

        services.AddHttpContextAccessor();
        services.AddScoped<IEnvironmentProvider, WebHostEnvironmentProvider>();
        services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}

