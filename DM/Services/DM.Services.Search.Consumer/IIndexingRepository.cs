using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.SearchEngine;

namespace DM.Services.Search.Consumer
{
    /// <summary>
    /// Wrapper over the ElasticSearch indexing mechanism
    /// </summary>
    public interface IIndexingRepository
    {
        /// <summary>
        /// Store searchable entities in index
        /// </summary>
        /// <param name="entities">Entities to store</param>
        /// <returns></returns>
        Task Index(params SearchEntity[] entities);

        /// <summary>
        /// Delete indexed document
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <returns></returns>
        Task Delete(Guid entityId);
    }
}