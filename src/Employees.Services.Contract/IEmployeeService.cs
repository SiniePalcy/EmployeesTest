using Employees.Domain.Model;
using Employees.Shared.Model;
using Employees.Shared.Requests;

namespace Employees.Services.Contract;

public interface IEmployeeService
{
    public Task<SystemLog> AddEmployeeAsync(AddEmployeeRequest request);
}