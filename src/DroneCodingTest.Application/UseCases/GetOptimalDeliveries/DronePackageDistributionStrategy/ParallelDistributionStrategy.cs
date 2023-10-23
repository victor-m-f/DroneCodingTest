using DroneCodingTest.Domain.Models;
using DroneCodingTest.Shared.Dtos;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;

public sealed class ParallelDistributionStrategy : IDronePackageDistributionStrategy
{
    public List<DroneTripDto> DistributePackages(List<Drone> drones, List<Location> locations)
    {
        var droneTrips = new List<DroneTripDto>();
        var sortedLocations = locations
            .OrderByDescending(y => y.PackageWeight)
            .ToList();
        var maxDroneWeight = drones.Max(d => d.MaxWeight);

        // Remove locations that no drone can serve
        for (var i = sortedLocations.Count - 1; i >= 0; i--)
        {
            if (sortedLocations[i].PackageWeight > maxDroneWeight)
            {
                sortedLocations.RemoveAt(i);
            }
        }

        foreach (var drone in drones)
        {
            droneTrips.Add(new DroneTripDto(new DroneDto(drone.Name, drone.MaxWeight)));
        }

        while (sortedLocations.Count > 0)
        {
            foreach (var droneTrip in droneTrips)
            {
                var currentTrip = new TripDto(droneTrip.Trips.Count + 1);

                foreach (var location in sortedLocations.ToList())
                {
                    if (currentTrip.TotalWeight + location.PackageWeight <= droneTrip.Drone!.MaxWeight)
                    {
                        currentTrip.AddLocation(new LocationDto(location.Name, location.PackageWeight));
                        sortedLocations.Remove(location);
                    }
                }

                if (currentTrip.TotalWeight > 0)
                {
                    droneTrip.Trips.Add(currentTrip);
                }
            }
        }

        return droneTrips;
    }
}
