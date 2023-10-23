namespace DroneCodingTest.Shared.Dtos;

public sealed class DroneDto
{
    public string Name { get; set; } = string.Empty;
    public int MaxWeight { get; set; }

    public DroneDto(string name, int maxWeight)
    {
        Name = name;
        MaxWeight = maxWeight;
    }

    public DroneDto()
    {
    }
}
