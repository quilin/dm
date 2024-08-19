using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;

namespace DM.Services.Community.BusinessProcesses.Polls.Reading;

/// <inheritdoc />
internal class PollReadingService(
    IPollReadingRepository repository,
    IDateTimeProvider dateTimeProvider,
    IIdentityProvider identityProvider) : IPollReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<Poll> polls, PagingResult paging)> Get(
        PagingQuery pagingQuery, bool onlyActive, CancellationToken cancellationToken)
    {
        var activeAt = onlyActive ? dateTimeProvider.Now : (DateTimeOffset?) null;
        var totalCount = await repository.Count(activeAt, cancellationToken);
        var pageSize = identityProvider.Current.Settings.Paging.EntitiesPerPage;
        var pagingData = new PagingData(pagingQuery, pageSize, (int) totalCount);
        var polls = await repository.Get(activeAt, pagingData, cancellationToken);
        return (polls, pagingData.Result);
    }

    /// <inheritdoc />
    public async Task<Poll> Get(Guid pollId, CancellationToken cancellationToken)
    {
        var poll = await repository.Get(pollId, cancellationToken);
        if (poll == null)
        {
            throw new HttpException(HttpStatusCode.Gone, "Poll not found");
        }

        return poll;
    }
}