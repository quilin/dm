using System;
using DM.Services.Core.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Gaming.Dto.Input
{
    /// <summary>
    /// Query to search games by
    /// </summary>
    public class GamesQuery : PagingQuery
    {
        /// <summary>
        /// Games should only be in this status
        /// </summary>
        public GameStatus? Status { get; set; }

        /// <summary>
        /// Games should contain this tag
        /// </summary>
        public Guid? TagId { get; set; }
    }
}