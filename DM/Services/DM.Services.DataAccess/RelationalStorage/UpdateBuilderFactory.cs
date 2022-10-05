using System;

namespace DM.Services.DataAccess.RelationalStorage;

/// <inheritdoc />
internal class UpdateBuilderFactory : IUpdateBuilderFactory
{
    /// <inheritdoc />
    public IUpdateBuilder<TEntity> Create<TEntity>(Guid id) where TEntity : class, new()
    {
        return new UpdateBuilder<TEntity>(id);
    }
}