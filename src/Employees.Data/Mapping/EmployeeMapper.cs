using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

internal class EmployeeMapper : BaseMapper<Employee, EmployeeEntity>
{
    public override Employee? ToModel(EmployeeEntity? entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Employee
        {
            Id = entity.Id,
            Title = entity.Title,
            Email = entity.Email,
            CreatedAt = entity.CreatedAt,
        };
    }

    public override EmployeeEntity? ToEntity(Employee? model)
    {
        if (model is null)
        {
            return null;
        }

        return new EmployeeEntity
        {
            Id = model.Id,
            Title = model.Title,
            Email = model.Email,
            CreatedAt = model.CreatedAt ?? DateTimeOffset.Now,
        };
    }
}
