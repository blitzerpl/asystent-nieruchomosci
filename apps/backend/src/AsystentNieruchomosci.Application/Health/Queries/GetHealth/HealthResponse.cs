namespace AsystentNieruchomosci.Application.Health.Queries.GetHealth;

public class HealthResponse
{
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Environment { get; set; } = string.Empty;
    public string? CorrelationId { get; set; }
}

