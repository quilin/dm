using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Common.BusinessProcesses.Uploads
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