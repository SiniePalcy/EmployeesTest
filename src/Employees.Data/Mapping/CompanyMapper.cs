using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

internal class CompanyMapper : BaseMapper<Company, CompanyEntity>
{
    private readonly IMapper<Employee, EmployeeEntity> _employeeMapper;
    public CompanyMapper(IMapper<Employee, EmployeeEntity> employeeMapper)
    {
        _employeeMapper = employeeMapper;
    }

    public override Company? ToModel(CompanyEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Company
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedAt = entity.CreatedAt,
            Employees = _employeeMapper.ToModels(entity.Employees)?.ToList(),
        };
    }

    public override CompanyEntity? ToEntity(Company? model)
    {
        if (model is null)
        {
            return null;
        }

        return new CompanyEntity
        {
            Id = model.Id,
            Name = model.Name,
            CreatedAt = model.CreatedAt ?? DateTimeOffset.Now,
            Employees = _employeeMapper.ToEntities(model.Employees)?.ToList()
        };
    }
}
