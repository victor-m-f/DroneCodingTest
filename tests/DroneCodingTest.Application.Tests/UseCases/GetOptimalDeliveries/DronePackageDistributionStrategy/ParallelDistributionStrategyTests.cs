using DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;
using DroneCodingTest.Domain.Models;
using FluentAssertions;

namespace DroneCodingTest.Application.Tests.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;

public sealed class ParallelDistributionStrategyTests
{
    private readonly ParallelDistributionStrategy _strategy;

    public ParallelDistributionStrategyTests()
    {
        _strategy = new ParallelDistributionStrategy();
    }

    [Fact]
    public void DistributePackages_ShouldRemoveLocations_NoDroneCanServe()
    {
        // Arrange
        var drones = new List<Drone> { new Drone("Drone1", 10), new Drone("Drone2", 20) };
        var locations = new List<Location> { new Location("Loc1", 30), new Location("Loc2", 10) };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        var allTrips = result.SelectMany(x => x.Trips).ToList();
        allTrips.Should().NotContain(trip => trip.Locations.Any(loc => loc.Name == "Loc1"));
    }

    [Fact]
    public void DistributePackages_ShouldUseAllDrones()
    {
        // Arrange
        var drones = new List<Drone> { new Drone("Drone1", 20), new Drone("Drone2", 20) };
        var locations = new List<Location> { new Location("Loc1", 10), new Location("Loc2", 10) };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        result.Count.Should().Be(2);
    }

    [Fact]
    public void DistributePackages_ShouldCreateMultipleTrips_WhenNecessary()
    {
        // Arrange
        var drones = new List<Drone> { new Drone("Drone1", 20) };
        var locations = new List<Location> { new Location("Loc1", 10), new Location("Loc2", 10), new Location("Loc3", 10) };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        result[0].Trips.Count.Should().Be(2);
    }

    [Fact]
    public void DistributePackages_ShouldDistributeAllLocations()
    {
        // Arrange
        var drones = new List<Drone> { new Drone("Drone1", 20), new Drone("Drone2", 20) };
        var locations = new List<Location> { new Location("Loc1", 10), new Location("Loc2", 10) };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        var allLocations = result.SelectMany(x => x.Trips).SelectMany(t => t.Locations).ToList();
        allLocations.Should().Contain(loc => loc.Name == "Loc1");
        allLocations.Should().Contain(loc => loc.Name == "Loc2");
    }

    [Fact]
    public void DistributePackages_ShouldNotExceedDroneWeightLimit_PerTrip()
    {
        // Arrange
        var drones = new List<Drone> { new Drone("Drone1", 20) };
        var locations = new List<Location> { new Location("Loc1", 10), new Location("Loc2", 15) };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        foreach (var droneTrip in result)
        {
            foreach (var trip in droneTrip.Trips)
            {
                trip.TotalWeight.Should().BeLessOrEqualTo(droneTrip.Drone!.MaxWeight);
            }
        }
    }

    [Fact]
    public void DistributePackages_ShouldKeepLoadingCapableDrones_EvenIfOthersAreFull()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("SmallDrone1", 5),
            new Drone("SmallDrone2", 5),
            new Drone("BigDrone", 100)
        };
        var locations = new List<Location>
        {
            new Location("Loc1", 4),
            new Location("Loc2", 4),
            new Location("Loc3", 4),
            new Location("Loc4", 4),
            new Location("Loc5", 4),
            new Location("Loc6", 4),
            new Location("Loc7", 4),
            new Location("Loc8", 4),
            new Location("Loc9", 4),
            new Location("Loc10", 4)
        };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        var bigDroneTrips = result.Find(x => x.Drone!.Name == "BigDrone")!.Trips;
        var smallDrone1Trips = result.Find(x => x.Drone!.Name == "SmallDrone1")!.Trips;
        var smallDrone2Trips = result.Find(x => x.Drone!.Name == "SmallDrone2")!.Trips;

        bigDroneTrips.Count.Should().Be(1);
        smallDrone1Trips.Count.Should().Be(1);
        smallDrone2Trips.Count.Should().Be(1);
    }

    [Fact]
    public void DistributePackages_DronesShouldKeepMakingNewTrips_EvenIfOthersCant()
    {
        // Arrange
        var drones = new List<Drone>
        {
            new Drone("SmallDrone1", 5),
            new Drone("SmallDrone2", 5),
            new Drone("BigDrone", 20)
        };
        var locations = new List<Location>
        {
            new Location("Loc1", 5),
            new Location("Loc2", 5),
            new Location("Loc3", 5),
            new Location("Loc4", 5),
            new Location("Loc5", 10),
            new Location("Loc6", 10),
            new Location("Loc7", 10),
            new Location("Loc8", 10),
            new Location("Loc9", 10),
            new Location("Loc10", 10)
        };

        // Act
        var result = _strategy.DistributePackages(drones, locations);

        // Assert
        var bigDroneTrips = result.Find(x => x.Drone!.Name == "BigDrone")!.Trips;
        var smallDrone1Trips = result.Find(x => x.Drone!.Name == "SmallDrone1")!.Trips;
        var smallDrone2Trips = result.Find(x => x.Drone!.Name == "SmallDrone2")!.Trips;

        bigDroneTrips.Count.Should().Be(3);
        smallDrone1Trips.Count.Should().Be(2);
        smallDrone2Trips.Count.Should().Be(2);
    }
}
