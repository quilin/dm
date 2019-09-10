using System;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <inheritdoc />
    public class UpdateBuilderFactory : IUpdateBuilderFactory
    {
        /// <inheritdoc />
        public IUpdateBuilder<TEntity> Create<TEntity>(Guid id) where TEntity : class, new() =>
            new UpdateBuilder<TEntity>(id);
    }
}