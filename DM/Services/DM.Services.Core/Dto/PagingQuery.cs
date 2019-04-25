namespace DM.Services.Core.Dto
{
    /// <summary>
    /// Query for paging
    /// </summary>
    public class PagingQuery
    {
        /// <summary>
        /// Skip previous entities
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Page must contain entity of given number
        /// </summary>
        public int? Number { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int? Size { get; set; }
    }
}