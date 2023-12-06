using Employees.Data.Contract;
using Employees.Domain.Model;
using Employees.Shared.Enums;
using Employees.Shared.Requests;
using Microsoft.Extensions.Logging;

namespace Employees.Services.Contract;

public class CompanyService : ICompanyService
{
    private readonly ILogger<CompanyService> _logger;
    private readonly ICompanyRepository _companyRepository;
    public CompanyService(
        ILogger<CompanyService> logger, 
        ICompanyRepository companyRepository)
    {
        _logger = logger;
        _companyRepository = companyRepository;
    }

    public async Task<SystemLog> AddCompanyAsync(AddCompanyRequest request)
    {
        var systemLog = await _companyRepository.AddAsync(new Company
        {
            Name = request.Company.Name,
            Employees = request.Company.Employees?.Select(x => new Employee
            {
                 Email = x.Email,
                 Title = Enum.Parse<EmployeeTitle>(x.Title, true)
            })?.ToList(),
        });

        _logger.LogInformation($"Added new Company: {{ Id: {systemLog.GetId()} }}");

        return systemLog;
    }
}
