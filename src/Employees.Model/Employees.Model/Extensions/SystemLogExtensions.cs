namespace Employees.Domain.Model;

public static class SystemLogExtensions
{
    public static int GetId(this SystemLog self)
    {
        return int.Parse(self.ChangeSet["Id"]);
    }
}
