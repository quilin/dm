using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <inheritdoc />
internal class PollCreatingService : IPollCreatingService
{
    private readonly IValidator<CreatePoll> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IPollFactory factory;
    private readonly IPollCreatingRepository repository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public PollCreatingService(
        IValidator<CreatePoll> validator,
        IIntentionManager intentionManager,
        IPollFactory factory,
        IPollCreatingRepository repository,
        IInvokedEventProducer producer)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.repository = repository;
        this.producer = producer;
    }
        
    /// <inheritdoc />
    public async Task<Poll> Create(CreatePoll createPoll)
    {
        await validator.ValidateAndThrowAsync(createPoll);
        intentionManager.ThrowIfForbidden(PollIntention.Create);

        var poll = factory.Create(createPoll);
        var result = await repository.Create(poll);
        await producer.Send(EventType.NewPoll, result.Id);

        return result;
    }
}