using System.Threading.Tasks;
using DM.Services.DataAccess.Eventing;

namespace DM.Services.Search.Implementation
{
    /// <summary>
    /// Certain indexer for search engine
    /// </summary>
    public interface IIndexer
    {
        /// <summary>
        /// Tells if this event entity can be indexed
        /// </summary>
        /// <param name="invokedEvent">Event</param>
        /// <returns>Can be indexed by this indexer</returns>
        bool CanIndex(InvokedEvent invokedEvent);

        /// <summary>
        /// Indexes event entity in search engine
        /// </summary>
        /// <param name="invokedEvent">Event</param>
        /// <returns></returns>
        Task Index(InvokedEvent invokedEvent);
    }
}