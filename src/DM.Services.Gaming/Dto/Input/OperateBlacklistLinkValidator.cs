using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class OperateBlacklistLinkValidator : AbstractValidator<OperateBlacklistLink>
{
    /// <inheritdoc />
    public OperateBlacklistLinkValidator(
        IUserRepository userRepository)
    {
        RuleFor(r => r.GameId)
            .NotEmpty().WithMessage(ValidationError.Empty);

        RuleFor(r => r.Login)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MustAsync(userRepository.UserExists).WithMessage(ValidationError.Invalid);
    }
}