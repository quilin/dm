using System;
using DM.Services.Core.Dto;

namespace DM.Services.Core.Extensions
{
    public static class PagingHelper
    {
        public static PagingData GetPaging(int totalEntitiesCount, int entityNumber, int pageSize)
        {
            return new PagingData
            {
                TotalPagesCount = (int) Math.Ceiling((decimal) totalEntitiesCount / pageSize),
                CurrentPage = (int) Math.Ceiling((decimal) entityNumber / pageSize),
                PageSize = pageSize,
                EntityNumber = Math.Min(Math.Max(1, entityNumber), totalEntitiesCount)
            };
        }
    }
}