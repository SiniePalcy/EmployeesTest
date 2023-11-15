namespace Employees.Shared.Model;

public class CompanyDto 
{
    public string Name { get; set; } = default!;
    public List<EmployeeDto>? Employees { get; set; }
}
