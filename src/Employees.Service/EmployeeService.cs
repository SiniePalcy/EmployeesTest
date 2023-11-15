using Employees.Data.Contract;
using Employees.Domain.Model;
using Employees.Shared.Enums;
using Employees.Shared.Model;
using Employees.Shared.Requests;

namespace Employees.Services.Contract;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<SystemLog> AddEmployeeAsync(AddEmployeeRequest request)
    {
        var result = await _employeeRepository.AddAsync(new Employee
        {
            Title = Enum.Parse<EmployeeTitle>(request.Employee.Title, true),
            Email = request.Employee.Email,
        }, request.Employee.CompanyIds)!;

        await _employeeRepository.SaveAsync();

        return result;
    }
}