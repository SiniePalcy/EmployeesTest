using Employees.Services.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection self)
    {
        self.AddScoped<ICompanyService, CompanyService>();
        self.AddScoped<IEmployeeService, EmployeeService>();
        return self;
    }
}
