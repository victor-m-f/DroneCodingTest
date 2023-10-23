using DroneCodingTest.Application.UseCases.Base;
using DroneCodingTest.Application.UseCases.GetOptimalDeliveries.DronePackageDistributionStrategy;
using DroneCodingTest.Domain.Models;
using DroneCodingTest.Shared.Requests;
using DroneCodingTest.Shared.Results;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries;

public sealed class GetOptimalDeliveriesUseCase : UseCaseBase, IGetOptimalDeliveriesUseCase
{
    public GetOptimalDeliveriesUseCase(ILogger<GetOptimalDeliveriesUseCase> logger)
        : base(logger)
    {
    }

    public async Task<GetOptimalDeliveriesOutput> Handle(GetOptimalDeliveriesInput input, CancellationToken cancellationToken)
    {
        LogStart(input);

        var inputValidationResult = input.Validate();
        if (inputValidationResult.IsInvalid)
        {
            var output = new GetOptimalDeliveriesOutput(HttpStatusCode.BadRequest, inputValidationResult.Errors.ToArray());
            LogFailure(output);
            return output;
        }

        List<Drone> drones = ParseDrones(input.InputFileLines[0]);
        List<Location> locations = ParseLocations(input.InputFileLines[1..]);

        var droneTrips = GetDistributionStrategy(input.TravelRules).DistributePackages(drones, locations);

        LogSuccess();
        return new GetOptimalDeliveriesOutput(HttpStatusCode.OK, new DroneDeliveryResult(droneTrips));
    }

    private static List<Drone> ParseDrones(string droneLine)
    {
        var drones = new List<Drone>();
        var elements = droneLine.Split(',');

        for (int i = 0; i < elements.Length; i += 2)
        {
            var name = elements[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
            if (int.TryParse(elements[i + 1].Trim().Replace("[", string.Empty).Replace("]", string.Empty), out var maxWeight))
            {
                drones.Add(new Drone(name, maxWeight));
            }
            else
            {
                throw new ArgumentException($"Invalid maximum weight for drone '{name}'. Expected a number but found '{elements[i + 1].Trim()}'.");
            }
        }

        return drones;
    }

    private static List<Location> ParseLocations(string[] locationLines)
    {
        var locations = new List<Location>();

        foreach (var line in locationLines)
        {
            var elements = line.Split(',');

            var name = elements[0].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
            if (int.TryParse(elements[1].Trim().Replace("[", string.Empty).Replace("]", string.Empty), out var maxWeight))
            {
                locations.Add(new Location(name, maxWeight));
            }
            else
            {
                throw new ArgumentException($"Invalid maximum weight for location '{name}'. Expected a number but found '{elements[1].Trim()}'.");
            }
        }

        return locations;
    }

    private static IDronePackageDistributionStrategy GetDistributionStrategy(TravelRules travelRules)
    {
        return travelRules switch
        {
            TravelRules.NoTravelTime => new SequentialDistributionStrategy(),
            TravelRules.HasTravelTime => new ParallelDistributionStrategy(),
            _ => throw new ArgumentException("Invalid distribution strategy")
        };
    }
}
