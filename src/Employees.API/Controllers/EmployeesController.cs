using Employees.Shared.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Employees.API.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}