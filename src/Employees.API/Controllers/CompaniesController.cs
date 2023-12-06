using Employees.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employees.API.Controllers
{
    [ApiController]
    [Route("companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompaniesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCompanyRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}