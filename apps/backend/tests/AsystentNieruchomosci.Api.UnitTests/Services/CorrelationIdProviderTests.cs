using AsystentNieruchomosci.Api.Services;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AsystentNieruchomosci.Api.UnitTests.Services;

public class CorrelationIdProviderTests
{
    [Fact]
    public void GetCorrelationId_WhenCorrelationIdExists_ShouldReturnCorrelationId()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContext.Items["CorrelationId"] = "test-correlation-id";

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var provider = new CorrelationIdProvider(httpContextAccessorMock.Object);

        // Act
        var result = provider.GetCorrelationId();

        // Assert
        Assert.Equal("test-correlation-id", result);
    }

    [Fact]
    public void GetCorrelationId_WhenHttpContextIsNull_ShouldReturnNull()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        var provider = new CorrelationIdProvider(httpContextAccessorMock.Object);

        // Act
        var result = provider.GetCorrelationId();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetCorrelationId_WhenCorrelationIdNotExists_ShouldReturnNull()
    {
        // Arrange
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();

        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var provider = new CorrelationIdProvider(httpContextAccessorMock.Object);

        // Act
        var result = provider.GetCorrelationId();

        // Assert
        Assert.Null(result);
    }
}
