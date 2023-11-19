using Giacom.CallDetails.WebApi.Client;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace Giacom.CallDetails.Tests.Integration;

public abstract class IntegrationTestsBase : IAssemblyFixture<TestingWebApplicationFactory>, IAsyncLifetime
{
    private readonly TestingWebApplicationFactory _factory;
    private readonly ITestOutputHelper _outputHelper;
    protected readonly CallDetailsClient WebApiClient;

    protected readonly IServiceScope Scope;

    protected IntegrationTestsBase(TestingWebApplicationFactory factory, ITestOutputHelper outputHelper)
    {
        _factory = factory;
        _outputHelper = outputHelper;
        Scope = factory.CreateScope();
        WebApiClient = GetWebApiClient();
    }
    
    protected TService GetRequiredService<TService>() where TService : notnull
    {
        return Scope.ServiceProvider.GetRequiredService<TService>();
    }

    private CallDetailsClient GetWebApiClient()
    {
        return new CallDetailsClient(_factory.CreateClient());
    }
    
    public Task InitializeAsync()
    {
        return _factory.ResetDatabase();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}