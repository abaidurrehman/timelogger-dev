using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Commands;
using Timelogger.Entities;
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
        public async Task<IActionResult> Get()
        {
            var projects = await _mediator.Send(new GetProjectsQuery());
            return Ok(projects);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddProject([FromBody] Project project)
        {
            await _mediator.Send(new ProjectCommand()
            {
                Id = project.Id,
                Name = project.Name,
                Status = project.Status,
                Deadline = project.Deadline,
            });

            return Ok(new ApiResponse { Message = "Project added successfully." });
        }
    }
}