using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <inheritdoc />
internal class UpdateGameValidator : AbstractValidator<UpdateGame>
{
    /// <inheritdoc />
    public UpdateGameValidator(
        IUserRepository userRepository)
    {
        When(g => g.Title != default, () =>
            RuleFor(g => g.Title)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(100).WithMessage(ValidationError.Long));

        When(g => g.SystemName != default, () =>
            RuleFor(g => g.SystemName)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(50).WithMessage(ValidationError.Long));

        When(g => g.SettingName != default, () =>
            RuleFor(g => g.SettingName)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MaximumLength(50).WithMessage(ValidationError.Long));

        When(g => g.Info != default, () =>
            RuleFor(g => g.Info)
                .NotEmpty().WithMessage(ValidationError.Empty)
                .MinimumLength(200).WithMessage(ValidationError.Short));

        When(g => g.AssistantLogin != default, () =>
            RuleFor(g => g.AssistantLogin)
                .MustAsync(userRepository.UserExists).WithMessage(ValidationError.Invalid));
    }
}