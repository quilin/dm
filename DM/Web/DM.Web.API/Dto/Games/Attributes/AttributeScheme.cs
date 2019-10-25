using System.Collections.Generic;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for game attribute scheme
    /// </summary>
    public class AttributeScheme
    {
        /// <summary>
        /// Scheme identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Scheme title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Scheme author (null if the scheme is public)
        /// </summary>
        public User Author { get; set; }

        /// <summary>
        /// Scheme constraints
        /// </summary>
        public IEnumerable<AttributeConstraint> Constraints { get; set; }
    }
}