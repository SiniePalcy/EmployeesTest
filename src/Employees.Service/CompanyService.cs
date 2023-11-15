﻿using Employees.Data.Contract;
using Employees.Domain.Model;
using Employees.Shared.Requests;

namespace Employees.Services.Contract;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<SystemLog> AddCompanyAsync(AddCompanyRequest request)
    {
        var systemLog = await _companyRepository.AddAsync(new Company
        {
            Name = request.Company.Name,
        });

        await _companyRepository.SaveAsync();

        return systemLog;
    }
}