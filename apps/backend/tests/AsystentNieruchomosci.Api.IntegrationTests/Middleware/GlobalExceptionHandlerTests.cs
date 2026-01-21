using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;

namespace AsystentNieruchomosci.Api.IntegrationTests.Middleware;

public class GlobalExceptionHandlerTests(WebApplicationFactory factory) : IClassFixture<WebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("/test/validation", HttpStatusCode.BadRequest, 400, "Validation Error", "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
    [InlineData("/test/not-found", HttpStatusCode.NotFound, 404, "Resource Not Found", "https://tools.ietf.org/html/rfc7231#section-6.5.4")]
    [InlineData("/test/argument-null", HttpStatusCode.BadRequest, 400, "Missing Required Argument", "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
    [InlineData("/test/argument", HttpStatusCode.BadRequest, 400, "Invalid Argument", "https://tools.ietf.org/html/rfc7231#section-6.5.1")]
    [InlineData("/test/unauthorized", HttpStatusCode.Unauthorized, 401, "Unauthorized", "https://tools.ietf.org/html/rfc7235#section-3.1")]
    [InlineData("/test/server-error", HttpStatusCode.InternalServerError, 500, "An error occurred while processing your request", "https://tools.ietf.org/html/rfc7231#section-6.6.1")]
    public async Task HandleException_ShouldReturnCorrectStatusCodeAndProblemDetails(
        string endpoint,
        HttpStatusCode expectedStatusCode,
        int expectedStatus,
        string expectedTitle,
        string expectedType)
    {
        // Act
        var response = await _client.GetAsync(endpoint);

        // Assert
        response.StatusCode.Should().Be(expectedStatusCode);
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        problemDetails.Should().NotBeNull();
        problemDetails!.Status.Should().Be(expectedStatus);
        problemDetails.Title.Should().Be(expectedTitle);
        problemDetails.Type.Should().Be(expectedType);
        problemDetails.Extensions.Should().ContainKey("correlationId");
        problemDetails.Extensions.Should().ContainKey("timestamp");
    }

    [Fact]
    public async Task ProblemDetails_ShouldContainCorrelationId()
    {
        // Arrange
        var correlationId = Guid.NewGuid().ToString();
        _client.DefaultRequestHeaders.Add("X-Correlation-Id", correlationId);

        // Act
        var response = await _client.GetAsync("/test/validation");
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        problemDetails.Should().NotBeNull();
        problemDetails!.Extensions.Should().ContainKey("correlationId");
        problemDetails.Extensions["correlationId"]!.ToString().Should().Be(correlationId);
    }

    [Theory]
    [InlineData("/test/validation")]
    [InlineData("/test/not-found")]
    [InlineData("/test/server-error")]
    public async Task ProblemDetails_InDevelopment_ShouldContainStackTrace(string endpoint)
    {
        // Act
        var response = await _client.GetAsync(endpoint);
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        problemDetails.Should().NotBeNull();
        problemDetails!.Extensions.Should().ContainKey("stackTrace");
        problemDetails.Extensions.Should().ContainKey("exceptionType");
    }

    [Fact]
    public async Task ProblemDetails_InDevelopment_WithInnerException_ShouldContainInnerException()
    {
        // Act
        var response = await _client.GetAsync("/test/inner-exception");
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        problemDetails.Should().NotBeNull();
        problemDetails!.Extensions.Should().ContainKey("innerException");
        problemDetails.Extensions["innerException"]!.ToString().Should().Be("Test inner exception");
    }
}
