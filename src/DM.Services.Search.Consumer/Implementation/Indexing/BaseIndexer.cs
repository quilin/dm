using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Search.Consumer.Implementation.Indexing;

/// <inheritdoc />
public abstract class BaseIndexer : IIndexer
{
    /// <summary>
    /// Event type that this indexer can process
    /// </summary>
    protected abstract EventType EventType { get; }

    /// <inheritdoc />
    public bool CanIndex(EventType eventType) => eventType == EventType;

    /// <inheritdoc />
    public abstract Task Index(InvokedEvent message);
}