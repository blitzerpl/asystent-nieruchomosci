using MediatR;

namespace AsystentNieruchomosci.Application.Health.Queries.GetHealth;

public record GetHealthQuery : IRequest<HealthResponse>;

