using System.Linq;
using DM.Services.Core.Dto.Attributes;
using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input
{
    /// <inheritdoc />
    public class UpdateAttributeSchemaValidator : AbstractValidator<UpdateAttributeSchema>
    {
        /// <inheritdoc />
        public UpdateAttributeSchemaValidator(
            IValidator<ListAttributeValue> listAttributeValueValidator)
        {
            RuleFor(s => s.SchemaId)
                .NotEmpty().WithMessage(ValidationError.Empty);

            RuleFor(s => s.Name)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(200).WithMessage(ValidationError.Long);
            RuleForEach(s => s.Specifications)
                .ChildRules(specs =>
                {
                    specs.RuleFor(s => s.Name)
                        .NotEmpty().WithMessage(ValidationError.Empty)
                        .MaximumLength(200).WithMessage(ValidationError.Long);
                    specs.RuleFor(s => s.Constraints)
                        .Custom((constraints, ctx) =>
                        {
                            switch (constraints)
                            {
                                case NumberAttributeConstraints numberConstraints:
                                    if (numberConstraints.MaxValue.HasValue && numberConstraints.MinValue.HasValue &&
                                        numberConstraints.MaxValue.Value < numberConstraints.MinValue.Value)
                                    {
                                        ctx.AddFailure(ValidationError.Invalid);
                                    }

                                    break;
                                case StringAttributeConstraints stringConstraints:
                                    if (stringConstraints.MaxLength <= 0)
                                    {
                                        ctx.AddFailure(nameof(StringAttributeConstraints.MaxLength),
                                            ValidationError.Invalid);
                                    }

                                    break;
                                case ListAttributeConstraints listConstraints:
                                    if (listConstraints.Values.Length == 0)
                                    {
                                        ctx.AddFailure(nameof(ListAttributeConstraints.Values),
                                            ValidationError.Invalid);
                                    }

                                    foreach (var validationFailure in listConstraints.Values
                                        .Select(listAttributeValueValidator.Validate)
                                        .Where(r => !r.IsValid)
                                        .SelectMany(r => r.Errors))
                                    {
                                        ctx.AddFailure(validationFailure);
                                    }

                                    break;
                            }
                        });
                });
        }
    }
}