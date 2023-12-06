using Employees.Domain.Model;
using Employees.Shared.Enums;

namespace Employees.Data.Contract;

public interface IEmployeeRepository
{
    Task<SystemLog?> AddAsync(Employee model, List<int> companyIds);
    Task<SystemLog> UpdateAsync(Employee model);
    Task DeleteAsync(int id);
    Task<IEnumerable<Employee>> GetAsync(List<int>? ids = null);
    Task SaveAsync();
}
