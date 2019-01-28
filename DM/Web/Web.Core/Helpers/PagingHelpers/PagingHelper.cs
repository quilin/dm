using System;

namespace DM.Web.Core.Helpers.PagingHelpers
{
    public static class PagingHelper
    {
        public static int GetTotalPages(int totalEntitiesCount, int pageSize)
        {
            return Math.Max((int)Math.Ceiling(totalEntitiesCount / (decimal)pageSize), 1);
        }

        public static PagingData GetPagingData(int totalEntitiesCount, int entityNumber, int pageSize)
        {
            return new PagingData
            {
                TotalPagesCount = (int)Math.Ceiling((decimal)totalEntitiesCount / pageSize),
                CurrentPage = (int)Math.Ceiling((decimal)entityNumber/ pageSize),
                PageSize = pageSize,
                EntityNumber = Math.Min(Math.Max(1, entityNumber), totalEntitiesCount)
            };
        }
    }
}