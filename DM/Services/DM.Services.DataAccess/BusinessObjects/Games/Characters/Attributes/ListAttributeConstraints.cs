using System.Linq;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    public class ListAttributeConstraints : AttributeConstraints
    {
        public ListAttributeValue[] Values { get; set; }

        public override string GetDefaultValue() => Values?.FirstOrDefault()?.Value ?? string.Empty;
    }

    public class ListAttributeValue
    {
        public string Value { get; set; }
        public int Modifier { get; set; }
    }
}