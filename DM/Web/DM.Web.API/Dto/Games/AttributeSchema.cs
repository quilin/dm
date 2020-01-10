using System.Collections.Generic;
using System.Data;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// DTO model for game attribute schema
    /// </summary>
    public class AttributeSchema
    {
        /// <summary>
        /// Schema identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Schema title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Schema author
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Schema type
        /// </summary>
        public SchemaType Type { get; set; }

        /// <summary>
        /// Schema specifications
        /// </summary>
        public IEnumerable<AttributeSpecification> Specifications { get; set; }
    }
}