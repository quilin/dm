using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class UpdateRoomClaimValidator : AbstractValidator<UpdateRoomClaim>
{
    /// <inheritdoc />
    public UpdateRoomClaimValidator()
    {
        RuleFor(c => c.ClaimId)
            .NotEmpty().WithMessage(ValidationError.Empty);

        RuleFor(c => c.Policy)
            .Must(c => c != RoomAccessPolicy.NoAccess).WithMessage(ValidationError.Invalid);
    }
}