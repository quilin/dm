using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Gaming.BusinessProcesses.Readers.Reading;
using DM.Services.Gaming.BusinessProcesses.Readers.Subscribing;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Gaming;

/// <inheritdoc />
internal class ReaderApiService : IReaderApiService
{
    private readonly IReadersReadingService readingService;
    private readonly IReadingSubscribingService subscribingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public ReaderApiService(
        IReadersReadingService readingService,
        IReadingSubscribingService subscribingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.subscribingService = subscribingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<User>> Get(Guid gameId)
    {
        var readers = await readingService.Get(gameId);
        return new ListEnvelope<User>(readers.Select(mapper.Map<User>));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> Subscribe(Guid gameId)
    {
        var reader = await subscribingService.Subscribe(gameId);
        return new Envelope<User>(mapper.Map<User>(reader));
    }

    /// <inheritdoc />
    public Task Unsubscribe(Guid gameId) => subscribingService.Unsubscribe(gameId);
}