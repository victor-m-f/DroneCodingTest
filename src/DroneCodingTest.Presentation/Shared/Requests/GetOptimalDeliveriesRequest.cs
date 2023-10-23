using DroneCodingTest.Shared.Attributes;
using Microsoft.AspNetCore.Http;

namespace DroneCodingTest.Shared.Requests;

public sealed class GetOptimalDeliveriesRequest
{
    [File(AllowedExtensions = new string[] { ".txt" })]
    public IFormFile? InputFile { get; set; }
    public TravelRules TravelRules { get; set; }
}
