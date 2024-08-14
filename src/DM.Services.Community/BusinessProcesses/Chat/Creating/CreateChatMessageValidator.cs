using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <inheritdoc />
internal class CreateChatMessageValidator : AbstractValidator<CreateChatMessage>
{
    /// <inheritdoc />
    public CreateChatMessageValidator()
    {
        RuleFor(c => c.Text)
            .NotEmpty().WithMessage(ValidationError.Empty);
    }
}