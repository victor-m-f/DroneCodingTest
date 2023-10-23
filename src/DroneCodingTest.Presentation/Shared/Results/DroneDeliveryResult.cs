using DroneCodingTest.Shared.Dtos;

namespace DroneCodingTest.Shared.Results;

public sealed class DroneDeliveryResult
{
    public List<DroneTripDto> DroneTrips { get; set; }

    public DroneDeliveryResult(List<DroneTripDto> droneTrips)
    {
        DroneTrips = droneTrips;
    }
}
