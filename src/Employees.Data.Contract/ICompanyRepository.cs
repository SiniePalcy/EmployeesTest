using Employees.Domain.Model;

namespace Employees.Data.Contract;

public interface ICompanyRepository
{
    Task<SystemLog> AddAsync(Company model);
    Task<SystemLog> UpdateAsync(Company model);
    Task DeleteAsync(int id);
    Task<IEnumerable<Company>> GetAsync(List<int>? ids = null);
    Task SaveAsync();
}
