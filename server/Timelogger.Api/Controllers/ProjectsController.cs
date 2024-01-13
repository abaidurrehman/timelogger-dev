using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Entities;

namespace Timelogger.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly TimeloggerDbContext _context;

        public ProjectsController(TimeloggerDbContext context)
        {
            _context = context;
        }

        // GET api/projects
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Projects);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> AddProject([FromBody] Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse { Message = "Project added successfully." });
        }
    }
}