using Employees.Infrastructure.Extensions;
using Employees.Services.Contract;
using Employees.Shared.Requests;
using Employees.Shared.Responses;
using MediatR;

namespace Employees.API.Handlers;

public class AddEmployeeRequestHandler : IRequestHandler<AddEmployeeRequest, IdResponse>
{
    private readonly IEmployeeService _employeeService;

    public AddEmployeeRequestHandler(
        IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IdResponse> Handle(AddEmployeeRequest request, CancellationToken cancellationToken)
    {
        var systemLog = await _employeeService.AddEmployeeAsync(request);
        return systemLog.FormIdResponse(); 
    }
}
