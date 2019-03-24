using System;
using System.Collections.Generic;
using DM.Services.Core.Dto;

namespace DM.Services.Common.Dto
{
    /// <summary>
    /// DTO model for comment
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Date of creation
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Date of last update
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public GeneralUser Author { get; set; }

        /// <summary>
        /// Users who liked this comment
        /// </summary>
        public IEnumerable<GeneralUser> Likes { get; set; }
    }
}