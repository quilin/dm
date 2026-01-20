using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Readers.Reading;
using DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class ReaderApiService(
    IReadersReadingService readingService,
    IReadingSubscribingService subscribingService,
    IMapper mapper) : IReaderApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<User>> Get(Guid gameId, CancellationToken cancellationToken)
    {
        var readers = await readingService.Get(gameId, cancellationToken);
        return new ListEnvelope<User>(readers.Select(mapper.Map<User>));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Subscribe(Guid gameId, CancellationToken cancellationToken)
    {
        var reader = await subscribingService.Subscribe(gameId, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(reader));
    }

    /// <inheritdoc />
    public Task Unsubscribe(Guid gameId, CancellationToken cancellationToken) =>
        subscribingService.Unsubscribe(gameId, cancellationToken);
}