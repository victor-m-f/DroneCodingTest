using MediatR;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries;

public interface IGetOptimalDeliveriesUseCase : IRequestHandler<GetOptimalDeliveriesInput, GetOptimalDeliveriesOutput>
{
}
