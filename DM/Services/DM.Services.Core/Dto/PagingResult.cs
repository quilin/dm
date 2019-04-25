using System;

namespace DM.Services.Core.Dto
{
    /// <summary>
    /// DTO model for paged data
    /// </summary>
    public class PagingResult
    {
        /// <summary>
        /// Total pages of certain size across the filtered entities
        /// </summary>
        public int TotalPagesCount { get; private set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Selected entity number
        /// </summary>
        public int EntityNumber { get; private set; }

        /// <summary>
        /// Create paging data
        /// </summary>
        /// <param name="totalEntitiesCount">Total entities count</param>
        /// <param name="entityNumber">Selected entity number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns></returns>
        public static PagingResult Create(int totalEntitiesCount, int entityNumber, int pageSize)
        {
            return new PagingResult
            {
                TotalPagesCount = (int)Math.Ceiling((decimal)totalEntitiesCount / pageSize),
                CurrentPage = Math.Max(1, (int)Math.Ceiling((decimal)entityNumber/ pageSize)),
                PageSize = pageSize,
                EntityNumber = Math.Min(Math.Max(1, entityNumber), totalEntitiesCount)
            };
        }
    }
}