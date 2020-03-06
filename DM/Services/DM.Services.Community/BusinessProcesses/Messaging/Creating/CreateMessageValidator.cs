using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating
{
    /// <inheritdoc />
    public class CreateMessageValidator : AbstractValidator<CreateMessage>
    {
        /// <inheritdoc />
        public CreateMessageValidator()
        {
            RuleFor(m => m.ConversationId)
                .NotEmpty().WithMessage(ValidationError.Empty);

            RuleFor(m => m.Text)
                .NotEmpty().WithMessage(ValidationError.Empty);
        }
    }
}