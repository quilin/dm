using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.DataAccess.RelationalStorage;

/// <summary>
/// Builder for atomic update operation
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IUpdateBuilder<TEntity>
    where TEntity : class, new()
{
    /// <summary>
    /// Add field update operation
    /// </summary>
    /// <param name="field">Field lambda</param>
    /// <param name="value">Field value</param>
    /// <typeparam name="TValue">Field value type</typeparam>
    /// <returns>Builder itself for chaining</returns>
    IUpdateBuilder<TEntity> Field<TValue>(Expression<Func<TEntity, TValue>> field, TValue value);

    /// <summary>
    /// Check if builder is empty
    /// </summary>
    /// <returns></returns>
    bool HasChanges();

    /// <summary>
    /// Delete entity
    /// </summary>
    /// <returns></returns>
    IUpdateBuilder<TEntity> Delete();

    /// <summary>
    /// Attach update to db context
    /// </summary>
    /// <returns>Identifier</returns>
    Guid AttachTo(DbContext dbContext);

    /// <summary>
    /// Save changes in mongodb
    /// </summary>
    /// <returns></returns>
    Task<Guid> UpdateFor(DmMongoClient mongoClient, bool upsert);
}