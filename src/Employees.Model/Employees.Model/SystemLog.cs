using Employees.Shared.Enums;

namespace Employees.Domain.Model;

public class SystemLog : BaseObject
{
    public ResourceType ResourceType { get; set; }
    public EventType Event { get; set; }
    public Dictionary<string, string> ChangeSet { get; set; } = default!;
    public string? Comment { get; set; }
}
