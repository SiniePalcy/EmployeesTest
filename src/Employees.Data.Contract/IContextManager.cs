namespace Employees.Data.Contract;

public interface IContextManager
{
    public Task EnsureContextIsUpAsync();
}
