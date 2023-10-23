using DroneCodingTest.Domain.Models;
using DroneCodingTest.Shared.Dtos;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;

public sealed class SequentialDistributionStrategy : IDronePackageDistributionStrategy
{
    public List<DroneTripDto> DistributePackages(List<Drone> drones, List<Location> locations)
    {
        var droneTrips = new List<DroneTripDto>();

        var sortedDrones = drones
            .OrderByDescending(x => x.MaxWeight)
            .ToList();

        var sortedLocations = locations
            .OrderByDescending(x => x.PackageWeight)
            .ToList();

        foreach (var drone in sortedDrones)
        {
            var droneTrip = new DroneTripDto(new DroneDto(drone.Name, drone.MaxWeight));
            var currentTrip = new TripDto(1);

            foreach (var location in sortedLocations.ToList())
            {
                if (location.PackageWeight > drone.MaxWeight)
                {
                    continue;
                }

                var placed = false;
                foreach (var trip in droneTrip.Trips)
                {
                    if (trip.TotalWeight + location.PackageWeight <= drone.MaxWeight)
                    {
                        trip.AddLocation(new LocationDto(location.Name, location.PackageWeight));
                        sortedLocations.Remove(location);
                        placed = true;
                        break;
                    }
                }

                if (!placed)
                {
                    currentTrip = new TripDto(droneTrip.Trips.Count + 1);
                    currentTrip.AddLocation(new LocationDto(location.Name, location.PackageWeight));
                    sortedLocations.Remove(location);
                    droneTrip.Trips.Add(currentTrip);
                }
            }

            droneTrips.Add(droneTrip);
        }

        return droneTrips;
    }
}
