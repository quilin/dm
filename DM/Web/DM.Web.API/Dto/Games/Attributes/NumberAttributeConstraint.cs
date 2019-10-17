namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for number constraints
    /// </summary>
    public class NumberAttributeConstraint : AttributeConstraint
    {
        /// <summary>
        /// Minimal possible value
        /// </summary>
        public int? MinValue { get; set; }

        /// <summary>
        /// Maximal possible value
        /// </summary>
        public int? MaxValue { get; set; }
    }
}