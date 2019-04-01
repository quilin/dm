using System.Threading.Tasks;
using DM.Services.DataAccess.Eventing;

namespace DM.Services.SearchEngine.Implementation
{
    /// <summary>
    /// Composite indexer for search engine
    /// </summary>
    public interface ICompositeIndexer
    {
        /// <summary>
        /// Index entity from its general event
        /// </summary>
        /// <param name="invokedEvent">Event</param>
        /// <returns></returns>
        Task Index(InvokedEvent invokedEvent);
    }
}