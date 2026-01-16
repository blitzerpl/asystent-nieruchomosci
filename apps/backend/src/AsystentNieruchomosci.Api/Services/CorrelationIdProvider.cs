using AsystentNieruchomosci.Application.Common.Interfaces;

namespace AsystentNieruchomosci.Api.Services;

public class CorrelationIdProvider(IHttpContextAccessor httpContextAccessor) : ICorrelationIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private const string CorrelationIdItemKey = "CorrelationId";

    public string? GetCorrelationId()
    {
        return _httpContextAccessor.HttpContext?.Items[CorrelationIdItemKey]?.ToString();
    }
}

