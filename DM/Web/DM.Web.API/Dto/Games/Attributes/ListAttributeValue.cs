namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for list attribute value
    /// </summary>
    public class ListAttributeValue
    {
        /// <summary>
        /// Value text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Value modifier
        /// </summary>
        public int? Modifier { get; set; }
    }
}