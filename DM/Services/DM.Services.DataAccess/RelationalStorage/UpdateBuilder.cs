using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <summary>
    /// Builder for atomic update operation
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class UpdateBuilder<TEntity> where TEntity : class, new()
    {
        private readonly Guid id;
        private readonly IList<(Expression<Func<TEntity, object>>, object)> fields;
        private readonly bool empty;

        /// <inheritdoc />
        public UpdateBuilder(Guid id)
        {
            this.id = id;
            fields = new List<(Expression<Func<TEntity, object>>, object)>();
        }

        private UpdateBuilder()
        {
            empty = true;
        }

        /// <summary>
        /// Add field update operation
        /// </summary>
        /// <param name="field">Field lambda</param>
        /// <param name="value">Field value</param>
        /// <returns></returns>
        public UpdateBuilder<TEntity> Field(Expression<Func<TEntity, object>> field, object value)
        {
            if (!empty)
            {
                fields.Add((field, value));
            }
            return this;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <returns></returns>
        public Guid Update(DbContext dbContext)
        {
            if (empty || !fields.Any())
            {
                return id;
            }

            var entity = new TEntity();
            var type = entity.GetType();
            type.GetProperty($"{type.Name}Id").SetValue(entity, id);
            dbContext.Set<TEntity>().Attach(entity);
            foreach (var (field, value) in fields)
            {
                SetPropertyValue(entity, field, value);
                dbContext.Entry(entity).Property(field).IsModified = true;
            }

            return id;
        }

        private static void SetPropertyValue(TEntity target,
            Expression<Func<TEntity, object>> memberLambda, object value)
        {
            MemberExpression memberExpression;
            switch (memberLambda.Body)
            {
                case MemberExpression body:
                    memberExpression = body;
                    break;
                case UnaryExpression unary:
                    memberExpression = (MemberExpression) unary.RemoveConvert();
                    break;
                default:
                    return;
            }

            var property = (PropertyInfo) memberExpression.Member;
            property.SetValue(target, value);
        }
        
        /// <summary>
        /// Creates builder with no entities
        /// </summary>
        /// <returns></returns>
        public static UpdateBuilder<TEntity> Empty() => new UpdateBuilder<TEntity>();
    }
}