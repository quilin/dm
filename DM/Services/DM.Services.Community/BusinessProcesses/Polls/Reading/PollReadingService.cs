using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <inheritdoc />
internal class PollReadingService : IPollReadingService
{
    private readonly IPollReadingRepository repository;
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public PollReadingService(
        IPollReadingRepository repository,
        IDateTimeProvider dateTimeProvider,
        IIdentityProvider identityProvider)
    {
        this.repository = repository;
        this.dateTimeProvider = dateTimeProvider;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<(IEnumerable<Poll> polls, PagingResult paging)> Get(PagingQuery pagingQuery, bool onlyActive)
    {
        var activeAt = onlyActive ? dateTimeProvider.Now : (DateTimeOffset?) null;
        var totalCount = await repository.Count(activeAt);
        var pageSize = identityProvider.Current.Settings.Paging.EntitiesPerPage;
        var pagingData = new PagingData(pagingQuery, pageSize, (int) totalCount);
        var polls = await repository.Get(activeAt, pagingData);
        return (polls, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Poll> Get(Guid pollId)
    {
        var poll = await repository.Get(pollId);
        if (poll == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Poll not found");
        }

        return poll;
    }
}