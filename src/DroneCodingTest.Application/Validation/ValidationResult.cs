namespace DroneCodingTest.Application.Validation;

public sealed class ValidationResult
{
    private readonly HashSet<string> _errors;
    public bool IsValid { get; set; }
    public bool IsInvalid => !IsValid;
    public IReadOnlyCollection<string> Errors => _errors;

    public ValidationResult()
    {
        IsValid = true;
        _errors = new HashSet<string>();
    }

    public void AddError(string error)
    {
        IsValid = false;
        _errors.Add(error);
    }
}
