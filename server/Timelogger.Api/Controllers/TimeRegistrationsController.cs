using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Commands;
using Timelogger.Dto;
using Timelogger.Queries;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class TimeRegistrationsController : Controller
    {
        private readonly IMediator _mediator;

        public TimeRegistrationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddTimeRegistration(
            [FromBody] TimeRegistrationDto timeRegistration)
        {
            if (timeRegistration.StartTime >= timeRegistration.EndTime)
                return BadRequest(new ApiResponse { Message = "End time should be greater than start time." });


            var command = new AddTimeRegistrationCommand
            {
                TimeRegistration = timeRegistration
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess) return Ok(new ApiResponse { Message = result.Message });

            return BadRequest(new ApiResponse
            {
                Message = result.Message,
                Errors = result.Errors
            });
        }

        [HttpGet("GetTimesForProject/{projectId}")]
        public async Task<ActionResult<IEnumerable<TimeRegistrationDto>>> GetTimesForProject(int projectId)
        {
            var timeRegistrations = await _mediator.Send(new GetTimeRegistrationQueryQuery { ProjectId = projectId });

            return Ok(timeRegistrations);
        }
    }
}