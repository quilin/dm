namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    public class StringAttributeConstraints : AttributeConstraints
    {
        public int MaxLength { get; set; }

        public override string GetDefaultValue() => string.Empty;
    }
}