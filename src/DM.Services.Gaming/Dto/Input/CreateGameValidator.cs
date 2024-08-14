using DM.Services.Core.Exceptions;
using DM.Services.Gaming.BusinessProcesses.Games.Shared;
using FluentValidation;

namespace DM.Services.Gaming.Dto.Input;

/// <summary>
/// Validator for game creation DTO model
/// </summary>
internal class CreateGameValidator : AbstractValidator<CreateGame>
{
    /// <inheritdoc />
    public CreateGameValidator(
        IUserRepository userRepository)
    {
        RuleFor(g => g.Title)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(100).WithMessage(ValidationError.Long);

        RuleFor(g => g.SystemName)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(50).WithMessage(ValidationError.Long);

        RuleFor(g => g.SettingName)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MaximumLength(50).WithMessage(ValidationError.Long);

        RuleFor(g => g.Info)
            .NotEmpty().WithMessage(ValidationError.Empty)
            .MinimumLength(200).WithMessage(ValidationError.Short);

        When(g => !string.IsNullOrEmpty(g.AssistantLogin), () =>
            RuleFor(g => g.AssistantLogin)
                .MustAsync(userRepository.UserExists).WithMessage(ValidationError.Invalid));
    }
}