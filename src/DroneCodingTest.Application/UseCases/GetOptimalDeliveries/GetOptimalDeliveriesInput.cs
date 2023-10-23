using DroneCodingTest.Application.UseCases.Base;
using DroneCodingTest.Application.Validation;
using DroneCodingTest.Shared.Requests;
using System.Text;

namespace DroneCodingTest.Application.UseCases.GetOptimalDeliveries;

public class GetOptimalDeliveriesInput : InputBase<GetOptimalDeliveriesOutput>
{
    private string[]? _inputFileLines;

    public byte[] InputFile { get; }
    public string[] InputFileLines
    {
        get
        {
            if (_inputFileLines == null)
            {
                string content = Encoding.UTF8.GetString(InputFile);
                var base64Content = content.Replace("data:text/plain;base64,", string.Empty);
                byte[] decodedByteArray = Convert.FromBase64String(base64Content);
                string decodedString = Encoding.UTF8.GetString(decodedByteArray);
                _inputFileLines = decodedString.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return _inputFileLines;
        }
    }
    public TravelRules TravelRules { get; set; }

    public GetOptimalDeliveriesInput(byte[] inputFile, TravelRules travelRules)
    {
        InputFile = inputFile;
        TravelRules = travelRules;
    }

    public override ValidationResult Validate()
    {
        var validationResult = new ValidationResult();

        if (InputFileLines.Length < 2)
        {
            validationResult.AddError("Insufficient data lines. At least one line for drones and one line for locations are required.");
        }

        foreach (var line in InputFileLines)
        {
            var elements = line.Split(',');
            if (elements.Length % 2 != 0)
            {
                validationResult.AddError("Each line should contain even elements. Name and weight should be present for all drones and locations.");
            }

            for (int i = 1; i < elements.Length; i += 2)
            {
                var cleanedElement = elements[i].Trim().Replace("[", string.Empty).Replace("]", string.Empty);
                if (!int.TryParse(cleanedElement, out _))
                {
                    validationResult.AddError($"Expected a number but found '{cleanedElement}' at index {i + 1}. Weight values should be numeric.");
                }
            }
        }

        return validationResult;
    }
}
