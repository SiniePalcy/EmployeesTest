using Serilog;

namespace Employees.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSerilog(this IServiceCollection self, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
        self.AddLogging(builder => builder.AddSerilog());
        return self;
    }

    public static IServiceCollection RegisterMediatR(this IServiceCollection self)
    {
        self.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(Program).Assembly));
        return self;
    }
}
