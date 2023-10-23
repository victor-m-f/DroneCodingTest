using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DroneCodingTest.Server.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    public class ApiControllerBase : ControllerBase
    {
        private readonly ILogger _logger;
        protected IMediator Mediator { get; }

        public ApiControllerBase(ILogger logger, IMediator mediator)
        {
            _logger = logger;
            Mediator = mediator;
        }

        protected IDisposable StartUseCaseScope(string useCaseName) =>
            _logger.BeginScope(new Dictionary<string, string> { { "UseCase", useCaseName } });
    }
}
