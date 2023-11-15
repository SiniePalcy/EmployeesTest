namespace Employees.Domain.Model;

public class Company : BaseObject
{
    public string Name { get; set; } = default!;
    public List<Employee>? Employees { get; set; }
}
