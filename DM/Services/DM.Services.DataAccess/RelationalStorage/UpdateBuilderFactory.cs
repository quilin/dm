using System;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <inheritdoc />
    public class UpdateBuilderFactory : IUpdateBuilderFactory
    {
        /// <inheritdoc />
        public IUpdateBuilder<TEntity> Create<TEntity>(Guid id, bool toDelete = false) where TEntity : class, new() =>
            new UpdateBuilder<TEntity>(id, toDelete);
    }
}