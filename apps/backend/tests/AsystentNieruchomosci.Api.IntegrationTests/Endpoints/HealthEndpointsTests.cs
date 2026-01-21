using AsystentNieruchomosci.Application.Health.Queries.GetHealth;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace AsystentNieruchomosci.Api.IntegrationTests.Endpoints;

public class HealthEndpointsTests(WebApplicationFactory factory) : IClassFixture<WebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetHealth_ShouldReturnOk_WithHealthResponse()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var healthResponse = await response.Content.ReadFromJsonAsync<HealthResponse>();
        healthResponse.Should().NotBeNull();
        healthResponse!.Status.Should().Be("Healthy");
        healthResponse.Environment.Should().NotBeNullOrEmpty();
        healthResponse.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task GetHealth_ShouldReturnCorrelationId_WhenProvided()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        _client.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);

        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var healthResponse = await response.Content.ReadFromJsonAsync<HealthResponse>();
        healthResponse.Should().NotBeNull();
        healthResponse!.CorrelationId.Should().Be(correlationId);
    }

    [Fact]
    public async Task GetHealth_ShouldReturnCorrelationId_WhenNotProvided()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var healthResponse = await response.Content.ReadFromJsonAsync<HealthResponse>();
        healthResponse.Should().NotBeNull();
        healthResponse!.CorrelationId.Should().NotBeNullOrEmpty();
    }
}
