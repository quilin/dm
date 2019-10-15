namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for game attribute constraint
    /// </summary>
    public abstract class AttributeConstraint
    {
        /// <summary>
        /// Constraint title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The attribute is required
        /// </summary>
        public bool? Required { get; set; }
    }
}