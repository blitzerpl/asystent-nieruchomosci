using AsystentNieruchomosci.Application.Common.Interfaces;

namespace AsystentNieruchomosci.Api.Services;

public class WebHostEnvironmentProvider(IWebHostEnvironment environment) : IEnvironmentProvider
{
    private readonly IWebHostEnvironment _environment = environment;

    public string EnvironmentName => _environment.EnvironmentName;
}

