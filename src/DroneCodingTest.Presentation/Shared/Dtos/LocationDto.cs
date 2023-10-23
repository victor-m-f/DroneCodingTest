namespace DroneCodingTest.Shared.Dtos;

public sealed class LocationDto
{
    public string Name { get; set; }
    public int PackageWeight { get; set; }

    public LocationDto(string name, int packageWeight)
    {
        Name = name;
        PackageWeight = packageWeight;
    }
}
