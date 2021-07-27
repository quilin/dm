using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Uploading.Dto
{
    /// <inheritdoc />
    public class CreateUploadValidator : AbstractValidator<CreateUpload>
    {
        /// <inheritdoc />
        public CreateUploadValidator()
        {
            RuleFor(u => u.FileName)
                .NotEmpty().WithMessage(ValidationError.Empty);
        }
    }
}