using MediatR;
using AsystentNieruchomosci.Application.Common.Interfaces;

namespace AsystentNieruchomosci.Application.Health.Queries.GetHealth;

public class GetHealthQueryHandler : IRequestHandler<GetHealthQuery, HealthResponse>
{
    private readonly IEnvironmentProvider _environmentProvider;
    private readonly ICorrelationIdProvider _correlationIdProvider;

    public GetHealthQueryHandler(
        IEnvironmentProvider environmentProvider,
        ICorrelationIdProvider correlationIdProvider)
    {
        _environmentProvider = environmentProvider;
        _correlationIdProvider = correlationIdProvider;
    }

    public Task<HealthResponse> Handle(GetHealthQuery request, CancellationToken cancellationToken)
    {
        var response = new HealthResponse
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Environment = _environmentProvider.EnvironmentName,
            CorrelationId = _correlationIdProvider.GetCorrelationId()
        };

        return Task.FromResult(response);
    }
}

