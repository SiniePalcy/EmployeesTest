﻿using Employees.Data.Contract;
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
        IMapper<Employee, EmployeeEntity> mapper,
        IMapper<SystemLog, SystemLogEntity> systemLogMapper)
        : base(context, mapper, systemLogMapper, Shared.Enums.ResourceType.Employee)
    {
    }

    public async Task<SystemLog?> AddAsync(Employee model, List<int> companyIds)
    {
        var existingUser = await _context
            .Employees
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (existingUser is not null)
        {
            throw new ObjectAlreadyExistsException("email", existingUser.Email, typeof(EmployeeEntity));
        }

        var companies = _context
            .Companies
            .Include(x => x.Employees)
            .Where(x => companyIds.Contains(x.Id));
        if (!companies.Any())
        {
            throw new ObjectNotFoundException(
                companyIds.Select(x => ("id" + x, (object)x)).ToList(), 
                typeof(CompanyEntity));
        }

        bool addedEntry = false;
        SystemLog? result = null;
        foreach(var company in companies)
        {
            if (!company.Employees!.Any(x => x.Title == model.Title))
            {
                if (!addedEntry)
                {
                    result = await base.AddAsync(model);
                    addedEntry = true;
                }

                company.Employees!.Add(_mapper.ToEntity(model)!);
            }
        }

        if (!addedEntry)
        {
            throw new Exception($"Employee can't be added: all companies already have the employee '{model.Title}'");
        }

        return result;
    }

    public override Task<IEnumerable<Employee>> GetAsync(List<int>? ids = null)
    {
        if (ids is null)
        {
            return Task.FromResult(_context.Employees.Select(_mapper.ToModel))!;
        }
        else
        {
            return Task.FromResult(_context
                .Employees
                .Where(x => ids.Contains(x.Id))
                .Select(_mapper.ToModel))!;
        }
        
    }
}