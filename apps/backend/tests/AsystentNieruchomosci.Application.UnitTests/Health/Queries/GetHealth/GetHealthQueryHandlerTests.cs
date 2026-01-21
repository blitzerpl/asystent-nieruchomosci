using AsystentNieruchomosci.Application.Common.Interfaces;
using AsystentNieruchomosci.Application.Health.Queries.GetHealth;
using FluentAssertions;
using Moq;

namespace AsystentNieruchomosci.Application.UnitTests.Health.Queries.GetHealth;

public class GetHealthQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnHealthResponse_WithCorrectData()
    {
        // Arrange
        var environmentProviderMock = new Mock<IEnvironmentProvider>();
        environmentProviderMock.Setup(x => x.EnvironmentName).Returns("Development");

        var correlationIdProviderMock = new Mock<ICorrelationIdProvider>();
        correlationIdProviderMock.Setup(x => x.GetCorrelationId()).Returns("test-correlation-id");

        var handler = new GetHealthQueryHandler(
            environmentProviderMock.Object,
            correlationIdProviderMock.Object);

        var query = new GetHealthQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Healthy");
        result.Environment.Should().Be("Development");
        result.CorrelationId.Should().Be("test-correlation-id");
        result.Timestamp.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task Handle_ShouldReturnHealthResponse_WhenCorrelationIdIsNull()
    {
        // Arrange
        var environmentProviderMock = new Mock<IEnvironmentProvider>();
        environmentProviderMock.Setup(x => x.EnvironmentName).Returns("Production");

        var correlationIdProviderMock = new Mock<ICorrelationIdProvider>();
        correlationIdProviderMock.Setup(x => x.GetCorrelationId()).Returns((string?)null);

        var handler = new GetHealthQueryHandler(
            environmentProviderMock.Object,
            correlationIdProviderMock.Object);

        var query = new GetHealthQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Healthy");
        result.Environment.Should().Be("Production");
        result.CorrelationId.Should().BeNull();
    }
}
