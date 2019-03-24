namespace DM.Services.Core.Dto
{
    /// <summary>
    /// DTO model for paged data
    /// </summary>
    public class PagingData
    {
        /// <summary>
        /// Total pages of certain size across the filtered entities
        /// </summary>
        public int TotalPagesCount { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Selected entity number
        /// </summary>
        public int EntityNumber { get; set; }
    }
}