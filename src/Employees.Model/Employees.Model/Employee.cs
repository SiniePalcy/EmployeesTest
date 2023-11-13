using Employees.Doman.Model.Enums;

namespace Employees.Model;

public class Employee
{
    public EmployeeTitle Title { get; set; }
    public string Email { get; set; } = default!;
}