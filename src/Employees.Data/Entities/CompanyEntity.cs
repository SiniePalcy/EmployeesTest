namespace Employees.Data.Entities;

internal class CompanyEntity : BaseEntity
{
    public string Name { get; set; } = default!;
    public List<EmployeeEntity>? Employees { get; set; }
}
