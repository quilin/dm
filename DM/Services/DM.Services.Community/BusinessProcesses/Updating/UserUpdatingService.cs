using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Community.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.RelationalStorage;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Updating
{
    /// <inheritdoc />
    public class UserUpdatingService : IUserUpdatingService
    {
        private readonly IValidator<UpdateUser> validator;
        private readonly IUserReadingService userReadingService;
        private readonly IIntentionManager intentionManager;
        private readonly IUpdateBuilderFactory updateBuilderFactory;
        private readonly IUserUpdatingRepository repository;

        /// <inheritdoc />
        public UserUpdatingService(
            IValidator<UpdateUser> validator,
            IUserReadingService userReadingService,
            IIntentionManager intentionManager,
            IUpdateBuilderFactory updateBuilderFactory,
            IUserUpdatingRepository repository)
        {
            this.validator = validator;
            this.userReadingService = userReadingService;
            this.intentionManager = intentionManager;
            this.updateBuilderFactory = updateBuilderFactory;
            this.repository = repository;
        }

        /// <inheritdoc />
        public async Task<UserDetails> Update(UpdateUser updateUser)
        {
            await validator.ValidateAndThrowAsync(updateUser);
            var user = await userReadingService.Get(updateUser.Login);
            intentionManager.IsAllowed(UserIntention.Edit, user);

            var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
                .MaybeField(u => u.Status, updateUser.Status?.Trim())
                .MaybeField(u => u.Name, updateUser.Name?.Trim())
                .MaybeField(u => u.Location, updateUser.Location?.Trim())
                .MaybeField(u => u.Skype, updateUser.Skype?.Trim())
                .MaybeField(u => u.Info, updateUser.Info?.Trim())
                .MaybeField(u => u.RatingDisabled, updateUser.RatingDisabled);

            var settingsUpdate = updateBuilderFactory.Create<UserSettings>(user.UserId)
                .MaybeField(u => u.ColorSchema, updateUser.Settings?.ColorSchema)
                .MaybeField(u => u.NannyGreetingsMessage, updateUser.Settings?.NannyGreetingsMessage)
                .MaybeField(u => u.Paging.CommentsPerPage, updateUser.Settings?.Paging?.CommentsPerPage)
                .MaybeField(u => u.Paging.TopicsPerPage, updateUser.Settings?.Paging?.TopicsPerPage)
                .MaybeField(u => u.Paging.MessagesPerPage, updateUser.Settings?.Paging?.MessagesPerPage)
                .MaybeField(u => u.Paging.PostsPerPage, updateUser.Settings?.Paging?.PostsPerPage)
                .MaybeField(u => u.Paging.EntitiesPerPage, updateUser.Settings?.Paging?.EntitiesPerPage);

            await repository.Update(userUpdate, settingsUpdate);
            return await userReadingService.GetDetails(updateUser.Login);
        }
    }
}