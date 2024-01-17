using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Behaviors;
using Timelogger.Commands;
using Timelogger.Dto;
using Timelogger.Queries;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class TimeRegistrationController : BaseController
    {
        public TimeRegistrationController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddTimeRegistration(
            [FromBody] TimeRegistrationDto timeRegistration)
        {
            try
            {
                var addTimeRegistrationCommand = new AddTimeRegistrationCommand { TimeRegistration = timeRegistration };

                var result = await Mediator.Send(addTimeRegistrationCommand);
                if (result.IsSuccess)
                {
                    return Ok(new ApiResponse { Message = result.Message });
                }

                return BadRequest(new ApiResponse { Message = result.Message, Errors = result.Errors });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Message = "Validation failure",
                    Errors = ex.Failures.Select(failure => failure.ErrorMessage).ToList()
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse { Message = "An error occurred while processing the request." });
            }
        }

        [HttpGet("GetTimesForProject/{projectId}")]
        public async Task<ActionResult<IEnumerable<TimeRegistrationDto>>> GetTimesForProject(int projectId)
        {
            try
            {
                var getTimeRegistrationQueryQuery = new GetTimeRegistrationQueryQuery { ProjectId = projectId };

                var timeRegistrations = await Mediator.Send(getTimeRegistrationQueryQuery);
                return Ok(timeRegistrations);
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse { Message = "An error occurred while processing the request." });
            }
        }
    }
}