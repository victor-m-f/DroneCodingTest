using System.Net;

namespace DroneCodingTest.Application.UseCases.Base;

public class OutputBase
{
    public bool IsValid { get; protected set; }
    public HttpStatusCode HttpStatusCode { get; set; }
    public List<string> Errors { get; } = new();
    public bool IsInvalid => !IsValid;
    public int StatusCode => (int)HttpStatusCode;

    public OutputBase(HttpStatusCode httpStatusCode)
    {
        IsValid = true;
        HttpStatusCode = httpStatusCode;
    }

    public OutputBase(HttpStatusCode httpStatusCode, params string[] errorMessages)
    {
        IsValid = false;
        HttpStatusCode = httpStatusCode;
        AddErrors(errorMessages);
    }

    public void AddError(string errorMessage)
    {
        IsValid = false;

        Errors.Add(StandartizeErrorMessage(errorMessage));
    }

    public void AddErrors(IEnumerable<string> errorMessages)
    {
        IsValid = false;

        Errors.AddRange(errorMessages.Select(x => StandartizeErrorMessage(x)));
    }

    private static string StandartizeErrorMessage(string errorMessage)
    {
        errorMessage = errorMessage.Trim();

        if (!errorMessage.EndsWith("."))
        {
            errorMessage += ".";
        }

        return errorMessage;
    }
}
