using Employees.Data;
using Employees.Data.Contract;
using Employees.Services.Contract;
using Employees.Services.Extensions;
using Employees.Shared.Requests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Services.Tests;

public class CompanyServiceTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly ServiceProvider _serviceProvider;

    public CompanyServiceTests(DatabaseFixture fixture)
    {
        _fixture = fixture;

        var services = new ServiceCollection();
        services.AddScoped(sp =>
            _fixture.ServiceProvider.GetRequiredService<DataContext>());
        services.AddScoped(sp =>
            _fixture.ServiceProvider.GetRequiredService<IEmployeeRepository>());
        services.AddScoped(sp =>
            _fixture.ServiceProvider.GetRequiredService<ICompanyRepository>());
        services.AddLogging();
        services.RegisterServices();
        _serviceProvider = services.BuildServiceProvider();
    }

    [Fact]
    public async Task AddEmptyCompanyTest_ShouldBePassed()
    {
        string companyName = "Horns and Hooves";
        AddCompanyRequest request = new()
        {
            Company = new()
            {
                Name = companyName,
            }
        };

        var repository = _serviceProvider.GetRequiredService<ICompanyRepository>();
        var service = _serviceProvider.GetRequiredService<ICompanyService>();

        var log = await service.AddCompanyAsync(request);

        log.Should().NotBeNull();
        var companies = await repository.GetAsync(new List<int> { int.Parse(log.ChangeSet["Id"]) });
        companies.Should().OnlyContain(c => string.Equals(c.Name, companyName, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public Task AddCompanyWithEmployess_ShouldBePassed()
    {
        var request = new AddCompanyRequest
        {


        };

        var service = _serviceProvider.GetRequiredService<ICompanyService>();

        return Task.CompletedTask;
    }

}