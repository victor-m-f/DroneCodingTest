using DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;
using DroneCodingTest.Domain.Models;
using FluentAssertions;

namespace DroneCodingTest.Application.Tests.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy
{
    public sealed class SequentialDistributionStrategyTests
    {
        private readonly SequentialDistributionStrategy _sequentialDistributionStrategy;

        public SequentialDistributionStrategyTests()
        {
            _sequentialDistributionStrategy = new SequentialDistributionStrategy();
        }

        [Fact]
        public void DistributePackages_ShouldAssignMaxWeightDronesFirst()
        {
            // Arrange
            var drones = new List<Drone>
            {
                new Drone("Drone1", 50),
                new Drone("Drone2", 100)
            };

            var locations = new List<Location>
            {
                new Location("Location1", 30),
                new Location("Location2", 70)
            };

            // Act
            var result = _sequentialDistributionStrategy.DistributePackages(drones, locations);

            // Assert
            result[0].Drone!.MaxWeight.Should().Be(100);
            result[1].Drone!.MaxWeight.Should().Be(50);
        }

        [Fact]
        public void DistributePackages_ShouldSkipLocations_WhenPackageWeightExceedsDroneCapacity()
        {
            // Arrange
            var drones = new List<Drone> { new Drone("Drone1", 50) };
            var locations = new List<Location> { new Location("Location1", 60) };

            // Act
            var result = _sequentialDistributionStrategy.DistributePackages(drones, locations);

            // Assert
            result[0].Trips.Should().BeEmpty();
        }

        [Fact]
        public void DistributePackages_ShouldCreateMultipleTrips_WhenDroneCapacityIsExceeded()
        {
            // Arrange
            var drones = new List<Drone> { new Drone("Drone1", 50) };
            var locations = new List<Location>
            {
                new Location("Location1", 20),
                new Location("Location2", 20),
                new Location("Location3", 20)
            };

            // Act
            var result = _sequentialDistributionStrategy.DistributePackages(drones, locations);

            // Assert
            result[0].Trips.Count.Should().Be(2);
        }

        [Fact]
        public void DistributePackages_ShouldDistributeAllPossibleLocations()
        {
            // Arrange
            var drones = new List<Drone>
            {
                new Drone("Drone1", 50),
                new Drone("Drone2", 100)
            };

            var locations = new List<Location>
            {
                new Location("Location1", 30),
                new Location("Location2", 70),
                new Location("Location2", 300)
            };

            // Act
            var result = _sequentialDistributionStrategy.DistributePackages(drones, locations);

            // Assert
            var totalLocations = 0;
            foreach (var droneTrip in result)
            {
                foreach (var trip in droneTrip.Trips)
                {
                    totalLocations += trip.Locations.Count;
                }
            }

            totalLocations.Should().Be(2);
        }
    }
}
