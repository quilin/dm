using System.Threading;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Polls.Reading;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <inheritdoc />
internal class PollCreatingService(
    IValidator<CreatePoll> validator,
    IIntentionManager intentionManager,
    IPollFactory factory,
    IPollCreatingRepository repository,
    IInvokedEventProducer producer) : IPollCreatingService
{
    /// <inheritdoc />
    public async Task<Poll> Create(CreatePoll createPoll, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(createPoll, cancellationToken);
        intentionManager.ThrowIfForbidden(PollIntention.Create);

        var poll = factory.Create(createPoll);
        var result = await repository.Create(poll, cancellationToken);
        await producer.Send(EventType.NewPoll, result.Id);

        return result;
    }
}