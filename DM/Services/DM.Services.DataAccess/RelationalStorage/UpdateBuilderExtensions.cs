using System;
using System.Linq.Expressions;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <summary>
    /// Common update builder extensions
    /// </summary>
    public static class UpdateBuilderExtensions
    {
        /// <summary>
        /// Update field conditionally, if the value is not default
        /// </summary>
        /// <param name="updateBuilder"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static IUpdateBuilder<TEntity> MaybeField<TEntity>(this IUpdateBuilder<TEntity> updateBuilder,
            Expression<Func<TEntity, object>> field, string value) where TEntity : class, new() =>
            value == default ? updateBuilder : updateBuilder.Field(field, value);

        /// <summary>
        /// Update field conditionally, if the value is not default
        /// </summary>
        /// <param name="updateBuilder"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IUpdateBuilder<TEntity> MaybeField<TEntity, TValue>(this IUpdateBuilder<TEntity> updateBuilder,
            Expression<Func<TEntity, object>> field, TValue? value)
            where TEntity : class, new()
            where TValue : struct =>
                !value.HasValue ? updateBuilder : updateBuilder.Field(field, value.Value);
    }
}