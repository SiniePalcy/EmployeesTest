using Employees.Data;
using Employees.Data.Contract;
using Employees.Infrastructure.Exceptions;
using Employees.Services.Contract;
using Employees.Services.Extensions;
using Employees.Shared.Enums;
using Employees.Shared.Requests;
using Employees.Domain.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Employees.Services.Tests.TestContext;

namespace Employees.Services.Tests;

[Collection(CollectionNames.MainCollection)]
public class EmployeeServiceTests 
{
    private readonly DatabaseFixture _fixture;
    private readonly ServiceProvider _serviceProvider;

    public EmployeeServiceTests(DatabaseFixture fixture)
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
    public async Task AddEmployeeTest_WithoutCompany_ShouldThrowsException()
    {
        AddEmployeeRequest request = new()
        {
            Employee = new()
            {
                CompanyIds = new(),
                Email= "dev@domain.com",
                Title = "Developer",
            }
        };

        var service = _serviceProvider.GetRequiredService<IEmployeeService>();

        await Assert.ThrowsAsync<ObjectNotFoundException>(() => 
            service.AddEmployeeAsync(request)
        );
    }

    [Fact]
    public async Task AddEmployeeTest_ShouldBePassed()
    {
        var employeeService = _serviceProvider.GetRequiredService<IEmployeeService>();
        var companyService = _serviceProvider.GetRequiredService<ICompanyService>();
        var employeeRepository = _serviceProvider.GetRequiredService<IEmployeeRepository>();
        var companyRepository = _serviceProvider.GetRequiredService<ICompanyRepository>();

        AddCompanyRequest newCompanyRequest = new()
        {
            Company = new()
            {
                Name = "Foo",
            }
        };

        var companyLog = await companyService.AddCompanyAsync(newCompanyRequest);
        var companyId = companyLog.GetId();

        AddEmployeeRequest newEmployeeRequest = new()
        {
            Employee = new()
            {
                CompanyIds = new() { companyId },
                Email = "dev@domain.com",
                Title = "Developer",
            }
        };

        var employeeLog = await employeeService.AddEmployeeAsync(newEmployeeRequest);
        
        var employeeId = employeeLog.GetId();
        var employees = (await employeeRepository.GetAsync(new() { employeeId })).ToList();
        employees[0].Title.Should().Be(EmployeeTitle.Developer);
        employees[0].Email.Should().BeEquivalentTo("dev@domain.com");

        await employeeRepository.DeleteAsync(employeeId);
        await companyRepository.DeleteAsync(companyId);
    }
}