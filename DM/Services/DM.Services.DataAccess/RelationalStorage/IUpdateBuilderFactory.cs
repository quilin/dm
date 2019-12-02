using System;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <summary>
    /// Since builder is stateful pattern we need the factory to protect the state to be used in multiple places
    /// </summary>
    public interface IUpdateBuilderFactory
    {
        /// <summary>
        /// Create blank instance of update builder
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="toDelete">Delete entity</param>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <returns>Blank instance of update builder</returns>
        IUpdateBuilder<TEntity> Create<TEntity>(Guid id, bool toDelete = false) where TEntity : class, new();
    }
}