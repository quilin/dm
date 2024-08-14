using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class CreatePendingPostValidator : AbstractValidator<CreatePendingPost>
{
    /// <inheritdoc />
    public CreatePendingPostValidator(
        IUserRepository userRepository)
    {
        RuleFor(p => p.RoomId)
            .NotEmpty().WithMessage(ValidationError.Empty);
        RuleFor(p => p.PendingUserLogin)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MustAsync(userRepository.UserExists).WithMessage(ValidationError.Invalid);
    }
}