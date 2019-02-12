using DM.Services.Core.Dto;

namespace DM.Web.API.Dto.Contracts
{
    public class Paging
    {
        public Paging(PagingData pagingData)
        {
            Pages = pagingData.TotalPagesCount;
            Current = pagingData.CurrentPage;
            Size = pagingData.PageSize;
            Number = pagingData.EntityNumber;
        }
        
        public int Pages { get; }
        public int Current { get; }
        public int Size { get; }
        public int Number { get; }
    }
}