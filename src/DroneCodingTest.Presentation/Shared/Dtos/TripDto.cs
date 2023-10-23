namespace DroneCodingTest.Shared.Dtos;

public sealed class TripDto
{
    public string Name { get; set; } = string.Empty;
    public List<LocationDto> Locations { get; set; }
    public int TotalWeight { get; set; }

    public TripDto(int order)
        : this()
    {
        Name = $"Trip #{order}";

    }

    public TripDto()
    {
        Locations = new List<LocationDto>();
    }

    public void AddLocation(LocationDto location)
    {
        Locations.Add(location);
        TotalWeight += location.PackageWeight;
    }
}
