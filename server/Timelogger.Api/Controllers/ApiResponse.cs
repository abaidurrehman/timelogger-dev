using System.Collections.Generic;

namespace Timelogger.Api.Controllers
{
    public class ApiResponse
    {
        public string Message { get; set; }

        public List<string> Errors { get; set; }
    }
}