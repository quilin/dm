using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.DataAccess.RelationalStorage
{
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
        /// Attach update to db context
        /// </summary>
        /// <returns>Identifier</returns>
        Guid AttachTo(DbContext dbContext);
    }
}