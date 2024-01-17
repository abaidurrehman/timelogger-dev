using System;
using System.Collections.Generic;
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
    public class TimeRegistrationsController : BaseController
    {
        public TimeRegistrationsController(IMediator mediator) : base(mediator)
        {
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddTimeRegistration(
            [FromBody] TimeRegistrationDto timeRegistration)
        {
            try
            {
                var result =
                    await Mediator.Send(new AddTimeRegistrationCommand { TimeRegistration = timeRegistration });

                if (result.IsSuccess)
                {
                    return Ok(new ApiResponse { Message = result.Message });
                }

                return BadRequest(new ApiResponse { Message = result.Message, Errors = result.Errors });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ApiResponse { Message = string.Join(Environment.NewLine, ex.Failures) });
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
                var timeRegistrations =
                    await Mediator.Send(new GetTimeRegistrationQueryQuery { ProjectId = projectId });
                return Ok(timeRegistrations);
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse { Message = "An error occurred while processing the request." });
            }
        }
    }
}