namespace Employees.Shared.Model;

[Serializable]
public class ErrorDto
{
    public string Key { get; set; } = default!;
    public string Description { get; set; } = default!;
}
