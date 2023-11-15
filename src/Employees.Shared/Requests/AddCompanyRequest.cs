using Employees.Shared.Model;
using Employees.Shared.Responses;
using MediatR;

namespace Employees.Shared.Requests;

public class AddCompanyRequest : IRequest<IdResponse>
{
    public CompanyDto Company { get; set; } = default!;
}
