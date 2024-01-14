using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Entities;

namespace Timelogger.Data.Repositories
{
    public interface IProjectRepository
    {
        
        Task<IEnumerable<Project>> GetAllProjectsAsync(CancellationToken cancellationToken);

        Task<Project> GetProjectByIdAsync(int projectId, CancellationToken cancellationToken);
     
        Task<int> AddProjectAsync(Project project, CancellationToken cancellationToken);
    }
}