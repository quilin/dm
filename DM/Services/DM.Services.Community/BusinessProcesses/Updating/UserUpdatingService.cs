using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.Authorization;
using DM.Services.Community.BusinessProcesses.Reading;
using DM.Services.Community.Dto;
using DM.Services.DataAccess.BusinessObjects.Users;
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

            var updateBuilder = updateBuilderFactory.Create<User>(user.UserId);

            return await repository.Update(updateBuilder);
        }
    }
}