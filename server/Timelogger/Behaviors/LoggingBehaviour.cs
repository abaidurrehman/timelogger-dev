using System.Threading;
using System.Threading.Tasks;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Timelogger.Behaviors
{
    public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger _logger;

        public LoggingBehaviour(ILogger<TRequest> logger, IIdentityService identityService)
        {
            _logger = logger;

            _identityService = identityService;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            var userName = await _identityService.GetCurrentUserNameAsync();


            _logger.LogInformation("Request: {Name} {@UserName} {@Request}",
                requestName, userName, request);
        }
    }

    public interface IIdentityService
    {
        Task<string> GetCurrentUserNameAsync();
    }

    public class IdentityService : IIdentityService
    {
        public Task<string> GetCurrentUserNameAsync()
        {
            return Task.FromResult("Abaid");
        }
    }
}