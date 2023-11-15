using Employees.Data.Contract;

namespace Employees.API.Extensions;

public static class HostExtensions
{
    public static async Task EnsureContextIsUp(this IHost self)
    {
        using var serviceScope = self.Services.CreateScope();

        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<IHost>>();

        try
        {
            var contextManager =
                serviceScope.ServiceProvider.GetRequiredService<IContextManager>();

            await contextManager.EnsureContextIsUpAsync();

            logger.LogInformation($"Data context is ready");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Data context initialization is failed");
            throw;
        }
    }
}
