using System;
using DM.Services.Core.Dto;

namespace DM.Services.Core.Extensions
{
    /// <summary>
    /// Utils for paging
    /// </summary>
    public static class PagingHelper
    {
        /// <summary>
        /// Generates DTO model based on entities list information
        /// </summary>
        /// <param name="totalEntitiesCount">Total entities in list</param>
        /// <param name="entityNumber">Selected entity number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paging DTO</returns>
        public static PagingData GetPaging(int totalEntitiesCount, int entityNumber, int pageSize)
        {
            return new PagingData
            {
                TotalPagesCount = (int) Math.Ceiling((decimal) totalEntitiesCount / pageSize),
                CurrentPage = (int) Math.Ceiling((decimal) Math.Max(1, entityNumber) / pageSize),
                PageSize = pageSize,
                EntityNumber = Math.Min(Math.Max(1, entityNumber), totalEntitiesCount)
            };
        }
    }
}