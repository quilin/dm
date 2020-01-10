using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input
{
    /// <inheritdoc />
    public class ListAttributeValueValidator : AbstractValidator<ListAttributeValue>
    {
        /// <inheritdoc />
        public ListAttributeValueValidator()
        {
            RuleFor(a => a.Value)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(200).WithMessage(ValidationError.Long);
        }
    }
}