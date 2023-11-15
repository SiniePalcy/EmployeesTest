using Employees.Infrastructure.Serialization.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.Infrastructure.Serialization.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSerialization(this IServiceCollection self)
    {
        self.AddSingleton<IJsonSerializer, InternalJsonSerializer>();
        return self;
    }
}
