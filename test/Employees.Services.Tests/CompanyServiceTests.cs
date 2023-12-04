using Employees.Data;
using Employees.Data.Contract;
using Employees.Data.Repositories;
using Employees.Services.Contract;
using Employees.Services.Extensions;
using Employees.Shared.Enums;
using Employees.Shared.Requests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
        
        int addedId = int.Parse(log.ChangeSet["Id"]);
        var companies = await repository.GetAsync(new() { addedId });
        companies.Should().OnlyContain(c => string.Equals(c.Name, companyName, StringComparison.OrdinalIgnoreCase));

        await repository.DeleteAsync(addedId);
    }

    [Fact]
    public async Task AddCompanyWithEmployess_ShouldBePassed()
    {
        string companyName = "Horns and Hooves";
        var request = new AddCompanyRequest
        {
            Company = new()
            {
                Name = companyName,
                Employees = new()
                {
                    new()
                    {
                        Email = "employee1@domain.com",
                        Title = "Developer"
                    },
                    new()
                    {
                        Email = "employee2@domain.com",
                        Title = "Tester"
                    }
                }
            }

        };

        var service = _serviceProvider.GetRequiredService<ICompanyService>();
        var companyRepository = _serviceProvider.GetRequiredService<ICompanyRepository>();
        var employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();

        var log = await service.AddCompanyAsync(request);

        int addedId = int.Parse(log.ChangeSet["Id"]);
        var companies = await companyRepository.GetAsync();
        companies.Should().HaveCount(1);
        companies.First().Name.Should().BeEquivalentTo(companyName);

        var employees = (await employeeRepository.GetAsync()).ToList();
        employees.Should().HaveCount(2);
        employees[0].Email.Should().Be("employee1@domain.com");
        employees[0].Title.Should().Be(EmployeeTitle.Developer);
        employees[1].Email.Should().Be("employee2@domain.com");
        employees[1].Title.Should().Be(EmployeeTitle.Tester);

        employees.ForEach(async x => await employeeRepository.DeleteAsync(x.Id));
        await companyRepository.DeleteAsync(addedId);
    }

}