using DM.Services.Core.Exceptions;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Users.Updating;

/// <inheritdoc />
internal class UpdateUserValidator : AbstractValidator<UpdateUser>
{
    /// <inheritdoc />
    public UpdateUserValidator()
    {
        Unless(u => u.Status == null, () =>
            RuleFor(u => u.Status)
                .MaximumLength(200).WithMessage(ValidationError.Long));

        Unless(u => u.Name == null, () =>
            RuleFor(u => u.Name)
                .MaximumLength(100).WithMessage(ValidationError.Long));

        Unless(u => u.Location == null, () =>
            RuleFor(u => u.Location)
                .MaximumLength(100).WithMessage(ValidationError.Long));

        Unless(u => u.Icq == null, () =>
            RuleFor(u => u.Icq)
                .MaximumLength(20).WithMessage(ValidationError.Long));

        Unless(u => u.Skype == null, () =>
            RuleFor(u => u.Skype)
                .MaximumLength(50).WithMessage(ValidationError.Long));

        Unless(u => u.Settings == null, () =>
        {
            Unless(u => u.Settings.NannyGreetingsMessage == null, () =>
                RuleFor(u => u.Settings.NannyGreetingsMessage)
                    .NotEmpty().WithMessage(ValidationError.Empty));

            Unless(u => u.Settings.Paging == null, () =>
            {
                RuleFor(u => u.Settings.Paging.CommentsPerPage)
                    .GreaterThan(0).WithMessage(ValidationError.Invalid)
                    .LessThan(200).WithMessage(ValidationError.Invalid);

                RuleFor(u => u.Settings.Paging.MessagesPerPage)
                    .GreaterThan(0).WithMessage(ValidationError.Invalid)
                    .LessThan(200).WithMessage(ValidationError.Invalid);

                RuleFor(u => u.Settings.Paging.PostsPerPage)
                    .GreaterThan(0).WithMessage(ValidationError.Invalid)
                    .LessThan(200).WithMessage(ValidationError.Invalid);

                RuleFor(u => u.Settings.Paging.TopicsPerPage)
                    .GreaterThan(0).WithMessage(ValidationError.Invalid)
                    .LessThan(200).WithMessage(ValidationError.Invalid);

                RuleFor(u => u.Settings.Paging.EntitiesPerPage)
                    .GreaterThan(0).WithMessage(ValidationError.Invalid)
                    .LessThan(200).WithMessage(ValidationError.Invalid);
            });
        });
    }
}