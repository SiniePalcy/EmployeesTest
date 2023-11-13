using Employees.Doman.Model.Enums;

namespace Employees.Doman.Model;

public class SystemLog : BaseObject
{
    public ResourceType ResourceType { get; set; }
    public EventType Event { get; set; }
    public Dictionary<string, object> ChangeSet { get; set; } = default!;
    public string? Comment { get; set; }
}
