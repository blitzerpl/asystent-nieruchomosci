using AsystentNieruchomosci.Application.Common.Interfaces;

namespace AsystentNieruchomosci.Api.Services;

public class CorrelationIdProvider : ICorrelationIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CorrelationIdItemKey = "CorrelationId";

    public CorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCorrelationId()
    {
        return _httpContextAccessor.HttpContext?.Items[CorrelationIdItemKey]?.ToString();
    }
}

