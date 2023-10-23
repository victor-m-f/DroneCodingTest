using DroneCodingTest.Domain.Models;
using DroneCodingTest.Shared.Dtos;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy
{
    internal interface IDronePackageDistributionStrategy
    {
        public List<DroneTripDto> DistributePackages(List<Drone> drones, List<Location> locations);
    }
}
