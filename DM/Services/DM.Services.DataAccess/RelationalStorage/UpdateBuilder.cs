using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <summary>
    /// Builder for atomic update operation
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class UpdateBuilder<TEntity> where TEntity : class, new()
    {
        private readonly IList<(Expression<Func<TEntity, object>>, object)> fields;

        /// <inheritdoc />
        public UpdateBuilder()
        {
            fields = new List<(Expression<Func<TEntity, object>>, object)>();
        }

        /// <summary>
        /// Add field update operation
        /// </summary>
        /// <param name="field">Field lambda</param>
        /// <param name="value">Field value</param>
        /// <returns></returns>
        public UpdateBuilder<TEntity> Field(Expression<Func<TEntity, object>> field, object value)
        {
            fields.Add((field, value));
            return this;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <returns></returns>
        public Task Update(Guid id, DbContext dbContext)
        {
            var entity = new TEntity();
            var type = entity.GetType();
            type.GetProperty($"{type.Name}Id").SetValue(entity, id);
            dbContext.Set<TEntity>().Attach(entity);
            foreach (var (field, value) in fields)
            {
                SetPropertyValue(entity, field, value);
                dbContext.Entry(entity).Property(field).IsModified = true;
            }

            return dbContext.SaveChangesAsync();
        }

        private static void SetPropertyValue<T, TValue>(TEntity target,
            Expression<Func<T, TValue>> memberLambda, TValue value)
        {
            if (!(memberLambda.Body is MemberExpression memberSelectorExpression))
            {
                return;
            }

            var property = memberSelectorExpression.Member as PropertyInfo;
            if (property != null)
            {
                property.SetValue(target, value, null);
            }
        }
    }
}