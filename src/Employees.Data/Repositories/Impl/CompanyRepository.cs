using AutoMapper;
using Employees.Data.Contract;
using Employees.Data.Entities;
using Employees.Domain.Model;
using Employees.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Employees.Data.Repositories.Impl;

internal class CompanyRepository : BaseRepository<Company, CompanyEntity>, ICompanyRepository
{
    private readonly IEmployeeRepository _employeeRepository;
    public CompanyRepository(
        DataContext context,
        IMapper mapper,
        IEmployeeRepository employeeRepository)
        : base(context, mapper, Shared.Enums.ResourceType.Company)
    {
        _employeeRepository = employeeRepository;
    }

    public override Task<IEnumerable<Company>> GetAsync(List<int>? ids = null)
    {
        if (ids is null)
        {
            return Task.FromResult(_context.Companies.Select(_mapper.Map<Company>))!;
        }
        else
        {
            return Task.FromResult(_context
                .Companies
                .Where(x => ids.Contains(x.Id))
                .Select(_mapper.Map<Company>))!;
        }
    }

    public override async Task<SystemLog> AddAsync(Company model)
    {
        var existingCompany = await  _context
            .Companies
            .FirstOrDefaultAsync(x => x.Name == model.Name);

        if (existingCompany is not null)
        {
            throw new ObjectAlreadyExistsException("name", model.Name, typeof(CompanyEntity));
        }

        var result =  await base.AddAsync(model);
        int id = int.Parse(result.ChangeSet["Id"]);

        if (model.Employees is not null)
        {
            foreach(var employeeModel in model.Employees)
            {
                await _employeeRepository.AddAsync(employeeModel, new List<int> { id });
            }
        }

        return result;
    }
}
