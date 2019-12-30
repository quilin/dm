using System;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <summary>
    /// Factory for attribute schema DAL model
    /// </summary>
    public interface ISchemaFactory
    {
        /// <summary>
        /// Create schema DAL model from DTO model
        /// </summary>
        /// <param name="createAttributeSchema">DTO model</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        AttributeSchema Create(CreateAttributeSchema createAttributeSchema, Guid userId);

        /// <summary>
        /// Create schema DAL model for updating
        /// </summary>
        /// <param name="updateAttributeSchema">DTO model</param>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        AttributeSchema Create(UpdateAttributeSchema updateAttributeSchema, Guid userId);
    }
}