using Employees.Data.Contract;
using Employees.Domain.Model;
using Employees.Shared.Enums;
using Employees.Shared.Model;
using Employees.Shared.Requests;
using Microsoft.Extensions.Logging;

namespace Employees.Services.Contract;

public class EmployeeService : IEmployeeService
{
    private readonly ILogger<EmployeeService> _logger; 
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(
        ILogger<EmployeeService> logger,
        IEmployeeRepository employeeRepository)
    {
        _logger = logger;
        _employeeRepository = employeeRepository;
    }

    public async Task<SystemLog> AddEmployeeAsync(AddEmployeeRequest request)
    {
        var result = await _employeeRepository.AddAsync(new Employee
        {
            Title = Enum.Parse<EmployeeTitle>(request.Employee.Title, true),
            Email = request.Employee.Email,
        }, request.Employee.CompanyIds)!;

        _logger.LogInformation($"Added new Employee: {{ Id: {result!.ChangeSet["Id"]} }}");

        return result;
    }
}