using DroneCodingTest.Application.UseCases.Base;
using DroneCodingTest.Shared.Results;
using System.Net;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries;

public class GetOptimalDeliveriesOutput : OutputBase
{
    public DroneDeliveryResult? DroneDeliveryResult { get; }

    public GetOptimalDeliveriesOutput(HttpStatusCode httpStatusCode, DroneDeliveryResult droneDeliveryResult)
        : base(httpStatusCode)
    {
        DroneDeliveryResult = droneDeliveryResult;
    }

    public GetOptimalDeliveriesOutput(HttpStatusCode httpStatusCode, params string[] errorMessages)
        : base(httpStatusCode, errorMessages)
    {
    }
}
