using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Search.Consumer.Implementation.Indexing;
using Jamq.Client.Abstractions.Consuming;
using Microsoft.Extensions.Logging;

namespace DM.Services.Search.Consumer.Implementation;

/// <inheritdoc />
internal class CompositeIndexer : IProcessor<string, InvokedEvent>
{
    private readonly IEnumerable<IIndexer> indexers;
    private readonly ILogger<CompositeIndexer> logger;

    /// <inheritdoc />
    public CompositeIndexer(
        IEnumerable<IIndexer> indexers,
        ILogger<CompositeIndexer> logger)
    {
        this.indexers = indexers;
        this.logger = logger;
    }

    /// <inheritdoc />
    public async Task<ProcessResult> Process(string key, InvokedEvent message, CancellationToken cancellationToken)
    {
        await Task.WhenAll(indexers
            .Where(i => i.CanIndex(message.Type))
            .Select(i => i.Index(message)));
        logger.LogInformation("DM.Event {EventType} for entity {EntityId} is indexed",
            message.Type, message.EntityId);
        return ProcessResult.Success;
    }
}