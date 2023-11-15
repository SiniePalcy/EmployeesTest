using Employees.Shared.Model;
using Employees.Shared.Responses;
using MediatR;

namespace Employees.Shared.Requests;

public class AddEmployeeRequest : IRequest<IdResponse>
{
    public EmployeeDto Employee { get; set; } = default!; 
}
