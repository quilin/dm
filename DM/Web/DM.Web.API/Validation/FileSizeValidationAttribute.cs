using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace DM.Web.API.Validation;

/// <inheritdoc />
internal class FileSizeValidationAttribute : ValidationAttribute
{
    private readonly long maxSize;

    /// <inheritdoc />
    public FileSizeValidationAttribute(long maxSize)
    {
        this.maxSize = maxSize;
    }

    /// <inheritdoc />
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) => value switch
    {
        IFormFileCollection files => files.All(IsValid)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage),
        IFormFile file => IsValid(file)
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage),
        _ => new ValidationResult("Value must be a file")
    };

    private bool IsValid(IFormFile formFile) => formFile.Length <= maxSize;
}