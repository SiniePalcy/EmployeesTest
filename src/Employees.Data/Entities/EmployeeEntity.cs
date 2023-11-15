using Employees.Shared.Enums;

namespace Employees.Data.Entities;

internal class EmployeeEntity : BaseEntity
{
    public EmployeeTitle Title { get; set; }
    public string Email { get; set; } = default!;
    public List<CompanyEntity>? Companies { get; set; }
}
