using System;
using AttributeSchema = DM.Services.Gaming.Dto.Shared.AttributeSchema;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <summary>
/// Factory for attribute schema DAL model
/// </summary>
internal interface ISchemaFactory
{
    /// <summary>
    /// Create new DAL model for schema creating
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    DbAttributeSchema CreateNew(AttributeSchema schema, Guid userId);

    /// <summary>
    /// Create new DAL model for schema updating
    /// </summary>
    /// <param name="schema"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    DbAttributeSchema CreateToUpdate(AttributeSchema schema, Guid userId);
}