using Employees.Data.Contract;
using Employees.Data.Entities;
using Employees.Data.Mapping;
using Employees.Domain.Model;
using Employees.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Employees.Data.Repositories.Impl;

internal class CompanyRepository : BaseRepository<Company, CompanyEntity>, ICompanyRepository
{
    public CompanyRepository(
        DataContext context,
        IMapper<Company, CompanyEntity> mapper,
        IMapper<SystemLog, SystemLogEntity> systemLogMapper)
        : base(context, mapper, systemLogMapper, Shared.Enums.ResourceType.Company)
    { }

    public override Task<IEnumerable<Company>> GetAsync(List<int>? ids = null)
    {
        if (ids is null)
        {
            return Task.FromResult(_context.Companies.Select(_mapper.ToModel))!;
        }
        else
        {
            return Task.FromResult(_context
                .Companies
                .Where(x => ids.Contains(x.Id))
                .Select(_mapper.ToModel))!;
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

        return await base.AddAsync(model);
    }
}
