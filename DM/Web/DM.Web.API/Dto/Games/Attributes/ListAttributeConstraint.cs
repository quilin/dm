using System.Collections.Generic;

namespace DM.Web.API.Dto.Games.Attributes
{
    /// <summary>
    /// DTO model for list of values constraints
    /// </summary>
    public class ListAttributeConstraint : AttributeConstraint
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<ListAttributeValue> Values { get; set; }
    }
}