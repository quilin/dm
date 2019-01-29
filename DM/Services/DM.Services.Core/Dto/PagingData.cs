namespace DM.Services.Core.Dto
{
    public class PagingData
    {
        public int TotalPagesCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int EntityNumber { get; set; }
    }
}