using Employees.Domain.Model;
using Employees.Shared.Requests;

namespace Employees.Services.Contract;

public interface ICompanyService
{
    public Task<SystemLog> AddCompanyAsync(AddCompanyRequest request);
}
