namespace DroneCodingTest.Shared.Dtos;

public sealed class DroneTripDto
{
    public DroneDto? Drone { get; set; }
    public List<TripDto> Trips { get; set; }
    public int RemainingWeight => Drone.MaxWeight - Trips.Sum(x => x.TotalWeight);

    public DroneTripDto(DroneDto drone)
        : this()
    {
        Drone = drone;
    }

    public DroneTripDto()
    {
        Trips = new List<TripDto>();
    }
}
