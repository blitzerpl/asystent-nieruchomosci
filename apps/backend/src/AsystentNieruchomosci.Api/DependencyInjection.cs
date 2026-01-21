using AsystentNieruchomosci.Api.Configuration;
using AsystentNieruchomosci.Api.Middleware;
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
        services.AddSingleton<ICorrelationIdProvider, CorrelationIdProvider>();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.Configure<CorsSettings>(
            configuration.GetSection(CorsSettings.SectionName));

        services.AddCors(options =>
        {
            var corsSettings = configuration.GetSection(CorsSettings.SectionName)
                .Get<CorsSettings>();

            if (corsSettings?.AllowedOrigins?.Length > 0)
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsSettings.AllowedOrigins);

                    if (corsSettings.AllowedMethods?.Length > 0)
                    {
                        policy.WithMethods(corsSettings.AllowedMethods);
                    }

                    if (corsSettings.AllowedHeaders?.Length > 0)
                    {
                        policy.WithHeaders(corsSettings.AllowedHeaders);
                    }

                    if (corsSettings.AllowCredentials)
                    {
                        policy.AllowCredentials();
                    }
                });
            }
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}

