using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <inheritdoc />
    internal class UpdateBuilder<TEntity> : IUpdateBuilder<TEntity>
        where TEntity : class, new()
    {
        private readonly Guid id;
        private readonly bool toDelete;
        private readonly IList<Action<TEntity, DbContext>> updateActions;

        /// <inheritdoc />
        public UpdateBuilder(Guid id, bool toDelete = false)
        {
            this.id = id;
            this.toDelete = toDelete;
            updateActions = new List<Action<TEntity, DbContext>>();
        }

        /// <inheritdoc />
        public IUpdateBuilder<TEntity> Field<TValue>(Expression<Func<TEntity, TValue>> field, TValue value)
        {
            updateActions.Add((entity, dbContext) =>
            {
                SetPropertyValue(entity, field, value);
                dbContext.Entry(entity).Property(field).IsModified = true;
            });
            return this;
        }

        /// <inheritdoc />
        public Guid AttachTo(DbContext dbContext)
        {
            var entity = new TEntity();
            var type = entity.GetType();
            var propertyInfo = type.GetProperty($"{type.Name}Id");
            if (propertyInfo == null)
            {
                throw new UpdateBuilderException($"No key property was found for entity {type.Name}");
            }

            propertyInfo.SetValue(entity, id);

            if (toDelete)
            {
                dbContext.Set<TEntity>().Attach(entity);
                dbContext.Entry(entity).State = EntityState.Deleted;
                return id;
            }

            if (!updateActions.Any())
            {
                return id;
            }

            dbContext.Set<TEntity>().Attach(entity);
            foreach (var updateAction in updateActions)
            {
                updateAction.Invoke(entity, dbContext);
            }

            return id;
        }

        private static void SetPropertyValue<TValue>(TEntity target,
            Expression<Func<TEntity, TValue>> memberLambda, TValue value)
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
    }
}