namespace Employees.Data.Entities;

internal class BaseEntity : IEntity
{
    public int Id { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
