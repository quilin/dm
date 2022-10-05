using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Search.Consumer.Implementation.Indexing;

/// <summary>
/// Certain indexer for search engine
/// </summary>
internal interface IIndexer
{
    /// <summary>
    /// Tells if this event entity can be indexed
    /// </summary>
    /// <param name="eventType">Event type</param>
    /// <returns>Can be indexed by this indexer</returns>
    bool CanIndex(EventType eventType);

    /// <summary>
    /// Indexes event entity in search engine
    /// </summary>
    /// <param name="message">Event</param>
    /// <returns></returns>
    Task Index(InvokedEvent message);
}