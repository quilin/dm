using DM.Services.Core.Dto;

namespace DM.Web.API.Dto.Contracts
{
    /// <summary>
    /// Paging DTO model
    /// </summary>
    public class Paging
    {
        /// <inheritdoc />
        public Paging(PagingData pagingData)
        {
            Pages = pagingData.TotalPagesCount;
            Current = pagingData.CurrentPage;
            Size = pagingData.PageSize;
            Number = pagingData.EntityNumber;
        }

        /// <summary>
        /// Total pages count
        /// </summary>
        public int Pages { get; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int Current { get; }

        /// <summary>
        /// Page size
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Entity number
        /// </summary>
        public int Number { get; }
    }
}