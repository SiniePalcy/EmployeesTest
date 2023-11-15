namespace Employees.Domain.Model;

public class BaseObject
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
