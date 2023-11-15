namespace Employees.Shared.Model;

public class EmployeeDto
{
    public string Title { get; set; } = default!;
    public string Email { get; set; } = default!;
    public List<int> CompanyIds { get; set; } = default!;
}
