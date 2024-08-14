using System;
using System.Collections.Generic;
using System.Linq;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;

namespace DM.Services.Community.BusinessProcesses.Polls.Creating;

/// <inheritdoc />
internal class PollFactory : IPollFactory
{
    private readonly IGuidFactory guidFactory;
    private readonly IDateTimeProvider dateTimeProvider;

    /// <inheritdoc />
    public PollFactory(
        IGuidFactory guidFactory,
        IDateTimeProvider dateTimeProvider)
    {
        this.guidFactory = guidFactory;
        this.dateTimeProvider = dateTimeProvider;
    }

    /// <inheritdoc />
    public Poll Create(CreatePoll createPoll)
    {
        return new Poll
        {
            Id = guidFactory.Create(),
            StartDate = dateTimeProvider.Now.UtcDateTime,
            EndDate = createPoll.EndDate.UtcDateTime,
            Global = true,
            Title = createPoll.Title,
            Options = createPoll.Options
                .Select(o => new PollOption
                {
                    Id = guidFactory.Create(),
                    Text = o,
                    UserIds = new List<Guid>()
                })
                .ToList()
        };
    }
}