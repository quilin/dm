using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Publish;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating
{
    /// <inheritdoc />
    public class PollCreatingService : IPollCreatingService
    {
        private readonly IValidator<CreatePoll> validator;
        private readonly IIntentionManager intentionManager;
        private readonly IPollFactory factory;
        private readonly IPollCreatingRepository repository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public PollCreatingService(
            IValidator<CreatePoll> validator,
            IIntentionManager intentionManager,
            IPollFactory factory,
            IPollCreatingRepository repository,
            IInvokedEventPublisher publisher)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.factory = factory;
            this.repository = repository;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task<Poll> Create(CreatePoll createPoll)
        {
            await validator.ValidateAndThrowAsync(createPoll);
            intentionManager.ThrowIfForbidden(PollIntention.Create);

            var poll = factory.Create(createPoll);
            var result = await repository.Create(poll);
            await publisher.Publish(EventType.NewPoll, result.Id);

            return result;
        }
    }
}