namespace DroneCodingTest.Domain.Models;

public sealed class Drone
{
    public string Name { get; }
    public int MaxWeight { get; }

    public Drone(string name, int maxWeight)
    {
        Name = name;
        MaxWeight = maxWeight;
    }
}
