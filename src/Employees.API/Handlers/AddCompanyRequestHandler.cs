using Employees.Infrastructure.Extensions;
using Employees.Services.Contract;
using Employees.Shared.Requests;
using Employees.Shared.Responses;
using MediatR;

namespace Employees.API.Handlers;

public class AddCompanyRequestHandler : IRequestHandler<AddCompanyRequest, IdResponse>
{
    private readonly ICompanyService _companyService;

    public AddCompanyRequestHandler(
        ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IdResponse> Handle(AddCompanyRequest request, CancellationToken cancellationToken)
    {
        var systemLog = await _companyService.AddCompanyAsync(request);
        return systemLog.FormIdResponse();
    }
}
