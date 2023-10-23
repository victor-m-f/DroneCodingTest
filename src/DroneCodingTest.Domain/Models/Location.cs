namespace DroneCodingTest.Domain.Models;

public sealed class Location
{
    public string Name { get; }
    public int PackageWeight { get; }

    public Location(string name, int packageWeight)
    {
        Name = name;
        PackageWeight = packageWeight;
    }
}
