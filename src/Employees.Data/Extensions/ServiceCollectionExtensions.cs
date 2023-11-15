using Employees.Data.Contract;
using Employees.Data.Entities;
using Employees.Data.Mapping;
using Employees.Data.Repositories;
using Employees.Data.Repositories.Impl;
using Employees.Data.Services;
using Employees.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

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
        self.RegisterMappers();
        self.AddScoped<IEmployeeRepository, EmployeeRepository>();
        self.AddScoped<ICompanyRepository, CompanyRepository>();
        return self;
    }

    private static IServiceCollection RegisterMappers(this IServiceCollection self)
    {
        var mapperTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>)))
            .ToList();
        
        foreach (var mapperType in mapperTypes)
        {
            self.AddSingleton(mapperType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapper<,>)), mapperType);
        }

        return self;
    }
}
