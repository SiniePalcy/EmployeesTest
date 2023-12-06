using AutoMapper;
using Employees.Data.Contract;
using Employees.Data.Entities;
using Employees.Data.Mapping;
using Employees.Domain.Model;
using Employees.Infrastructure.Exceptions;
using System.Data.Entity;

namespace Employees.Data.Repositories.Impl;

internal class EmployeeRepository : BaseRepository<Employee, EmployeeEntity>, IEmployeeRepository
{
    public EmployeeRepository(
        DataContext context,
        IMapper mapper)
        : base(context, mapper, Shared.Enums.ResourceType.Employee)
    {
    }

    public async Task<SystemLog?> AddAsync(Employee model, List<int> companyIds)
    {
        var existingUser = _context
            .Employees
            .FirstOrDefault(x => x.Email == model.Email);
        if (existingUser is not null)
        {
            throw new ObjectAlreadyExistsException("email", existingUser.Email, typeof(EmployeeEntity));
        }

        var companies = _context
            .Companies
            .Include(x => x.Employees)
            .Where(x => companyIds.Contains(x.Id))
            .ToList();
        if (!companies.Any())
        {
            throw new ObjectNotFoundException(
                companyIds.Select(x => ("objectId_" + x, (object)x)).ToList(), 
                typeof(CompanyEntity));
        }

        bool addedEntry = false;
        SystemLog? result = null;
        EmployeeEntity? addedEmployee = null;
        foreach(var company in companies)
        {
            var existingEmployee = company
                .Employees?
                .FirstOrDefault(x => x.Title == model.Title);
            if (existingEmployee is null)
            {
                if (!addedEntry)
                {
                    result = await base.AddAsync(model);
                    addedEmployee = _context.Find<EmployeeEntity>(result.GetId())!;
                    addedEntry = true;
                }

                company.Employees ??= new List<EmployeeEntity>();

                company.Employees.Add(addedEmployee!);
                _context.Update(company);
            }
        }

        if (!addedEntry)
        {
            throw new Exception($"Employee can't be added: all companies already have the employee '{model.Title}'");
        }

        await _context.SaveChangesAsync();

        return result;
    }

    public override Task<IEnumerable<Employee>> GetAsync(List<int>? ids = null)
    {
        if (ids is null)
        {
            return Task.FromResult(_context.Employees.Select(_mapper.Map<Employee>))!;
        }
        else
        {
            return Task.FromResult(_context
                .Employees
                .Where(x => ids.Contains(x.Id))
                .Select(_mapper.Map<Employee>))!;
        }
        
    }
}
