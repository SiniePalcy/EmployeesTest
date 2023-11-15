using Employees.Data.Contract;

namespace Employees.Data.Services;

internal class ContextManager : IContextManager
{
    private readonly DataContext _context;

    public ContextManager(DataContext context)
    {
        _context = context;
    }

    public Task EnsureContextIsUpAsync() => _context.Database.EnsureCreatedAsync();
}
