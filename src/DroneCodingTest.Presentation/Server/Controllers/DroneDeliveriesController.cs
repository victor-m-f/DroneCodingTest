using DroneCodingTest.Application.UseCases.GetOptimalDeliveries;
using DroneCodingTest.Server.Controllers.Base;
using DroneCodingTest.Shared.Requests;
using DroneCodingTest.Shared.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DroneCodingTest.Server.Controllers;

public class DroneDeliveriesController : ApiControllerBase
{
    public DroneDeliveriesController(
        ILogger<DroneDeliveriesController> logger,
        IMediator mediator)
        : base(logger, mediator)
    {
    }

    [AllowAnonymous]
    [HttpPost("OptimalDeliveries", Name = nameof(GetOptimalDeliveries))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DroneDeliveryResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(List<string>))]
    public async Task<ActionResult<DroneDeliveryResult>> GetOptimalDeliveries(
        [FromForm] GetOptimalDeliveriesRequest request,
        CancellationToken cancellationToken)
    {
        using (StartUseCaseScope(nameof(GetOptimalDeliveries)))
        {
            var fileContent = new byte[request.InputFile!.Length];
            using (var memoryStream = new MemoryStream())
            {
                await request.InputFile!.CopyToAsync(memoryStream, cancellationToken);
                fileContent = memoryStream.ToArray();
            }

            var input = new GetOptimalDeliveriesInput(fileContent, request.TravelRules);
            var output = await Mediator.Send(input, cancellationToken);

            return output.IsValid ?
                Ok(output.DroneDeliveryResult) :
                StatusCode(output.StatusCode, output.Errors);
        }
    }
}