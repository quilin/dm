namespace DM.Services.Gaming.Dto.Output
{
    /// <summary>
    /// DTO model for character attribute
    /// </summary>
    public class CharacterAttribute
    {
        /// <summary>
        /// Attribute title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Attribute value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Attribute modifier
        /// </summary>
        public int? Modifier { get; set; }
    }
}