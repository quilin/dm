using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class CreateRoomClaimValidator : AbstractValidator<CreateRoomClaim>
{
    /// <inheritdoc />
    public CreateRoomClaimValidator(
        IUserRepository userRepository)
    {
        RuleFor(c => c.Policy)
            .Must(p => p != RoomAccessPolicy.NoAccess).WithMessage(ValidationError.Invalid);

        When(c => c.CharacterId.HasValue, () =>
            RuleFor(c => c.CharacterId.Value)
                .NotEmpty().WithMessage(ValidationError.Empty));

        When(c => !string.IsNullOrEmpty(c.ReaderLogin), () =>
        {
            RuleFor(c => c.ReaderLogin)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MustAsync(userRepository.UserExists).WithMessage(ValidationError.Invalid);
            RuleFor(c => c.Policy)
                .Must(c => c == RoomAccessPolicy.ReadOnly).WithMessage(ValidationError.Invalid);
        });
    }
}