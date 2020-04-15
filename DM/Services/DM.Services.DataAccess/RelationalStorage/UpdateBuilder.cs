using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <inheritdoc />
    internal class UpdateBuilder<TEntity> : IUpdateBuilder<TEntity>
        where TEntity : class, new()
    {
        private readonly Guid id;
        private readonly IList<Action<TEntity, DbContext>> efUpdateActions;
        private readonly IList<Func<UpdateDefinition<TEntity>>> mongoUpdateActions;
        private bool toDelete;

        /// <inheritdoc />
        public UpdateBuilder(Guid id)
        {
            this.id = id;
            efUpdateActions = new List<Action<TEntity, DbContext>>();
            mongoUpdateActions = new List<Func<UpdateDefinition<TEntity>>>();
        }

        /// <inheritdoc />
        public IUpdateBuilder<TEntity> Field<TValue>(Expression<Func<TEntity, TValue>> field, TValue value)
        {
            if (toDelete)
            {
                throw new UpdateBuilderException("Builder is configured to delete entity, you cannot modify it");
            }

            efUpdateActions.Add((entity, dbContext) =>
            {
                SetPropertyValue(entity, field, value);
                dbContext.Entry(entity).Property(field).IsModified = true;
            });
            mongoUpdateActions.Add(() => new UpdateDefinitionBuilder<TEntity>().Set(field, value));
            return this;
        }

        /// <inheritdoc />
        public bool HasChanges() => toDelete || efUpdateActions.Any();

        public IUpdateBuilder<TEntity> Delete()
        {
            if (efUpdateActions.Any())
            {
                throw new UpdateBuilderException("Builder is configured to update entity, you cannot delete it");
            }

            toDelete = true;
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

            if (!efUpdateActions.Any())
            {
                return id;
            }

            dbContext.Set<TEntity>().Attach(entity);
            foreach (var updateAction in efUpdateActions)
            {
                updateAction.Invoke(entity, dbContext);
            }

            return id;
        }

        public (Guid, UpdateDefinition<TEntity>) DefineUpdateTo(DmMongoClient mongoClient)
        {
            var entityType = typeof(TEntity);
            if (entityType.GetCustomAttribute<MongoCollectionNameAttribute>() == null)
            {
                throw new UpdateBuilderException($"Entity type {entityType.Name} is not a mongo collection type");
            }

            if (entityType.GetProperty("Id") == null)
            {
                throw new UpdateBuilderException($"Entity type {entityType.Name} has no external Id property");
            }

            return (id, new UpdateDefinitionBuilder<TEntity>().Combine(
                mongoUpdateActions.Select(a => a.Invoke())));
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
                    Expression expression = unary;
                    do expression = ((UnaryExpression) expression).Operand;
                    while (expression.NodeType == ExpressionType.Convert ||
                           expression.NodeType == ExpressionType.ConvertChecked);
                    memberExpression = (MemberExpression) expression;
                    break;
                default:
                    return;
            }

            var property = (PropertyInfo) memberExpression.Member;
            property.SetValue(target, value);
        }
    }
}