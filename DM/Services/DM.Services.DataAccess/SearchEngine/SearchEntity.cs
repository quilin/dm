using System;
using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.DataAccess.SearchEngine
{
    /// <summary>
    /// Indexed entity
    /// </summary>
    public class SearchEntity
    {
        /// <summary>
        /// Entity identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Entity type
        /// </summary>
        public SearchEntityType EntityType { get; set; }

        /// <summary>
        /// Text to index
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Authorized roles list
        /// </summary>
        public IEnumerable<UserRole> AuthorizedRoles { get; set; }

        /// <summary>
        /// Authorized user ids list
        /// </summary>
        public IEnumerable<Guid> AuthorizedUsers { get; set; }
    }
}