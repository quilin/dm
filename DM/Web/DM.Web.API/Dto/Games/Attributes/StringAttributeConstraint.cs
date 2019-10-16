namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for string constraints
    /// </summary>
    public class StringAttributeConstraint : AttributeConstraint
    {
        /// <summary>
        /// Maximal possible length
        /// </summary>
        public int? MaxLength { get; set; }
    }
}