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
        private readonly IList<(Expression<Func<TEntity, object>>, object)> fields;

        /// <inheritdoc />
        public UpdateBuilder(Guid id)
        {
            this.id = id;
            fields = new List<(Expression<Func<TEntity, object>>, object)>();
        }

        /// <inheritdoc />
        public IUpdateBuilder<TEntity> Field(Expression<Func<TEntity, object>> field, object value)
        {
            fields.Add((field, value));
            return this;
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <returns></returns>
        public Guid AttachTo(DbContext dbContext)
        {
            if (!fields.Any())
            {
                return id;
            }

            var entity = new TEntity();
            var type = entity.GetType();
            var propertyInfo = type.GetProperty($"{type.Name}Id");
            if (propertyInfo == null)
            {
                throw new UpdateBuilderException($"No key property was found for entity {type.Name}");
            }
            
            propertyInfo.SetValue(entity, id);
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
    }
}