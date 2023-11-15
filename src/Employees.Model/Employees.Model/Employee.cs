using Employees.Shared.Enums;

namespace Employees.Domain.Model;

public class Employee : BaseObject
{
    public EmployeeTitle Title { get; set; }
    public string Email { get; set; } = default!;
}