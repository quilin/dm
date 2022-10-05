using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DM.Services.DataAccess.MongoIntegration;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DM.Services.DataAccess.RelationalStorage;

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
        var primaryKeyProperty = GetPrimaryKeyProperty();
        if (primaryKeyProperty == null)
        {
            throw new UpdateBuilderException($"No key property was found for entity {type.Name}");
        }
        var attachedEntry = dbContext.Set<TEntity>().Local.FirstOrDefault(entry => id.Equals(primaryKeyProperty.GetValue(entry)));
        if (attachedEntry != null)
        {
            dbContext.Entry(attachedEntry).State = EntityState.Detached;
        }

        primaryKeyProperty.SetValue(entity, id);

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

    public async Task<Guid> UpdateFor(DmMongoClient mongoClient, bool upsert)
    {
        var entityType = typeof(TEntity);
        if (entityType.GetCustomAttribute<MongoCollectionNameAttribute>() == null)
        {
            throw new UpdateBuilderException($"Entity type {entityType.Name} is not a mongo collection type");
        }

        var primaryKeyProperty = GetPrimaryKeyProperty();

        if (!mongoUpdateActions.Any())
        {
            return id;
        }

        var updateDefinition = new UpdateDefinitionBuilder<TEntity>()
            .Combine(mongoUpdateActions.Select(a => a.Invoke()));
        var filterDefinition = new FilterDefinitionBuilder<TEntity>()
            .Eq(primaryKeyProperty.Name, id);
        await mongoClient.GetCollection<TEntity>()
            .UpdateOneAsync(filterDefinition, updateDefinition, new UpdateOptions {IsUpsert = upsert});

        return id;
    }

    private static PropertyInfo GetPrimaryKeyProperty()
    {
        var entityType = typeof(TEntity);
        var propertyInfos = entityType.GetProperties();
        var keyAttributedProperty =
            propertyInfos.Where(i => i.GetCustomAttribute<KeyAttribute>() != null).ToArray();
        if (keyAttributedProperty.Length == 1)
        {
            return keyAttributedProperty.First();
        }

        var idProperty = entityType.GetProperty("Id");
        if (idProperty != null)
        {
            return idProperty;
        }

        var conventionIdProperty = entityType.GetProperty($"{entityType.Name}Id");
        if (conventionIdProperty != null)
        {
            return conventionIdProperty;
        }

        throw new UpdateBuilderException(
            $"Entity {entityType.Name} has no primary key properties or only has composite key");
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