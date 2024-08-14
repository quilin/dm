using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Common.Extensions;

/// <summary>
/// Extension for helper methods to fill counters for entities
/// </summary>
public static class EntitiesCounterFillExtensions
{
    /// <summary>
    /// Fill counters fields for passed parent entities
    /// </summary>
    /// <param name="repository">Counters repository</param>
    /// <param name="entities">Entities to fill</param>
    /// <param name="userId">Reading user identifier</param>
    /// <param name="getId">Entity identifier mapper</param>
    /// <param name="counterField">Expression of the field to fill counter in</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Task FillParentCounters<TEntity>(this IUnreadCountersRepository repository,
        ICollection<TEntity> entities, Guid userId,
        Func<TEntity, Guid> getId, Expression<Func<TEntity, int>> counterField) =>
        FillCounters(entities, userId, getId, repository.SelectByParents, counterField);

    /// <summary>
    /// Fill counters fields for passed entities
    /// </summary>
    /// <param name="repository">Counters repository</param>
    /// <param name="entities">Entities to fill</param>
    /// <param name="userId">Reading user identifier</param>
    /// <param name="getId">Entity identifier mapper</param>
    /// <param name="counterField">Expression of the field to fill counter in</param>
    /// <param name="entryType">Unread entry type</param>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public static Task FillEntityCounters<TEntity>(this IUnreadCountersRepository repository,
        ICollection<TEntity> entities, Guid userId,
        Func<TEntity, Guid> getId, Expression<Func<TEntity, int>> counterField,
        UnreadEntryType entryType = UnreadEntryType.Message) =>
        FillCounters(entities, userId, getId, repository.SelectByEntities, counterField, entryType);

    private static async Task FillCounters<TEntity>(ICollection<TEntity> entities, Guid userId,
        Func<TEntity, Guid> getId, Func<Guid, UnreadEntryType, Guid[], Task<IDictionary<Guid, int>>> getCounters,
        Expression<Func<TEntity, int>> counterField, UnreadEntryType entryType = UnreadEntryType.Message)
    {
        if (counterField.Body is MemberExpression {Member: PropertyInfo property})
        {
            var counters = await getCounters(userId, entryType, entities.Select(getId).ToArray());
            foreach (var entity in entities)
            {
                property.SetValue(entity, counters[getId(entity)]);
            }
        }
    }
}