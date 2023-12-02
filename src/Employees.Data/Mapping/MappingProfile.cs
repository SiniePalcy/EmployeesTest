using AutoMapper;
using Employees.Data.Entities;
using Employees.Domain.Model;

namespace Employees.Data.Mapping;

internal class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyEntity>();
        CreateMap<CompanyEntity, Company>();
        CreateMap<Employee, EmployeeEntity>();
        CreateMap<EmployeeEntity, Employee>();
        CreateMap<SystemLogEntity, SystemLog>();
    }
}
