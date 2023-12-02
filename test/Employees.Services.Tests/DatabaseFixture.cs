﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Employees.Data.Extensions;
using Employees.Data;

namespace Employees.Services.Tests;

public class DatabaseFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; set; }

    public DatabaseFixture()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var hostEnvironmentMock = new Mock<IHostEnvironment>();
        hostEnvironmentMock
            .Setup(x => x.EnvironmentName)
            .Returns("Development");

        var services = new ServiceCollection();
        services.AddSingleton(hostEnvironmentMock.Object);
        services.AddSingleton<IConfiguration>(configuration);
        services.RegisterDbContext(
            hostEnvironmentMock.Object,
            configuration,
            "PostgresConnString");
        services.RegisterRepositories();
        services.AddLogging();
        ServiceProvider = services.BuildServiceProvider();

        var context = ServiceProvider.GetRequiredService<DataContext>();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        var context = ServiceProvider.GetRequiredService<DataContext>();
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}
