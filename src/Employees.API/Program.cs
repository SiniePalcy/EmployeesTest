using AutoMapper;
using Employees.API.Extensions;
using Employees.API.Middleware;
using Employees.API.Validation;
using Employees.Data.Extensions;
using Employees.Infrastructure.Serialization.Extensions;
using Employees.Services.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterSerialization();
builder.Services.RegisterMediatR();
builder.Services.RegisterDbContext(builder.Environment, builder.Configuration, "PostgresConnString");
builder.Services.RegisterRepositories();
builder.Services.RegisterServices();
builder.Services.RegisterSerilog(builder.Configuration);
builder.Services.AddSingleton<ExceptionMiddleware>();
builder.Services.AddSerilog((sp, logging) =>
{
    logging.ReadFrom.Configuration(builder.Configuration);
});


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddEmployeeRequestValidator>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.EnsureContextIsUp();

app.Run();
