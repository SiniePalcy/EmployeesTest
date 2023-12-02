namespace Employees.Data.Entities;

public interface IEntity
{
    int Id { get; set; }
    DateTimeOffset? CreatedAt { get; set; }
    bool IsDeleted { get; set; }
}