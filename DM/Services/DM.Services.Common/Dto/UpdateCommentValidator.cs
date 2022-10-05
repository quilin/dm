using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Common.Dto;

/// <summary>
/// Validator for commentary updating DTO model
/// </summary>
internal class UpdateCommentValidator : AbstractValidator<UpdateComment>
{
    /// <inheritdoc />
    public UpdateCommentValidator()
    {
        RuleFor(c => c.CommentId)
            .NotEmpty();
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage(ValidationError.Empty);
    }
}