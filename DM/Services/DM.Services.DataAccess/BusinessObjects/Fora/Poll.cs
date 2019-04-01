using System;
using System.Collections.Generic;
using DM.Services.DataAccess.MongoIntegration;

namespace DM.Services.DataAccess.BusinessObjects.Fora
{
    /// <summary>
    /// DAL model for poll
    /// </summary>
    [MongoCollectionName("Polls")]
    public class Poll
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Moment from
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Moment to
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Question text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Options
        /// </summary>
        public PollOption[] Options { get; set; }
    }

    /// <summary>
    /// DAL model for poll option
    /// </summary>
    public class PollOption
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Answer text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Voted users identifiers
        /// </summary>
        public List<Guid> UserIds { get; set; }
    }
}