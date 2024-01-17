using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelogger.Entities;

namespace Timelogger.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TimeloggerDbContext _context;

        public ProjectRepository(TimeloggerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync(CancellationToken cancellationToken)
        {
            return await _context.Projects.ToListAsync(cancellationToken);
        }

        public async Task<Project> GetProjectByIdAsync(int projectId, CancellationToken cancellationToken)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        public async Task<int> AddProjectAsync(Project project, CancellationToken cancellationToken)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);
            return project.Id;
        }
    }
}