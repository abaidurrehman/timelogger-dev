using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Timelogger.Api.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;

        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}