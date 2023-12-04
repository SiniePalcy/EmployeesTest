using Employees.Data.Contract;
using Employees.Data.Mapping;
using Employees.Data.Repositories.Impl;
using Employees.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Employees.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDbContext(
        this IServiceCollection self,
        IHostEnvironment environment,
        IConfiguration configuration,
        string connStringName)
    {
        self.AddDbContext<DataContext>(options =>
        {
            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
            options.UseNpgsql(configuration.GetConnectionString(connStringName));
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton);
        self.AddScoped<IContextManager, ContextManager>();

        return self;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection self)
    {
        self.RegisterAutomapper();
        self.AddScoped<IEmployeeRepository, EmployeeRepository>();
        self.AddScoped<ICompanyRepository, CompanyRepository>();
        return self;
    }

    private static IServiceCollection RegisterAutomapper(this IServiceCollection self)
    {
        self.AddAutoMapper(typeof(MappingProfile));
        return self;
    }
   
}
