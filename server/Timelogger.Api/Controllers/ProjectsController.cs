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
    public class ProjectsController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Get()
        {
            var projects = await _mediator.Send(new GetProjectsQuery());

            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddProject([FromBody] ProjectDto project)
        {
            await _mediator.Send(new ProjectCommand
            {
                Project = project
            });

            return Ok(new ApiResponse { Message = "Project added successfully." });
        }
    }
}