using Humanizer;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DroneCodingTest.Shared.Attributes;
public sealed class FileAttribute : ValidationAttribute
{
    public long MaxSize { get; set; } = 2 * 1024 * 1024;
    public string[] AllowedExtensions { get; set; } = Array.Empty<string>();

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file)
        {
            return GenerateValidationResult(validationContext, "{0} is required.");
        }

        if (file.Length == 0)
        {
            return GenerateValidationResult(validationContext, "{0} must not be empty.");
        }

        if (file.Length > MaxSize)
        {
            return GenerateValidationResult(validationContext, "{0} size must be less than {1} bytes.", MaxSize.ToString());
        }

        if (AllowedExtensions.Length > 0)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!Array.Exists(AllowedExtensions, ext => ext.ToLower() == extension))
            {
                return GenerateExtensionValidationResult(validationContext);
            }
        }

        return ValidationResult.Success;
    }

    private static ValidationResult GenerateValidationResult(
        ValidationContext validationContext,
        string errorMessage,
        params string[] erroMessageParams)
    {
        var propertyName = validationContext.MemberName.Humanize();

        return new ValidationResult(
            string.Format(errorMessage, propertyName, erroMessageParams),
            new[] { validationContext.MemberName! });
    }

    private ValidationResult GenerateExtensionValidationResult(ValidationContext validationContext)
    {
        return new ValidationResult(
            string.Format("Invalid file extension. Allowed extensions: {0}.", string.Join(", ", AllowedExtensions)),
            new[] { validationContext.MemberName! });
    }
}
