namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    public class NumberAttributeConstraints : AttributeConstraints
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }

        public override string GetDefaultValue() => (MinValue ?? MaxValue ?? 0).ToString();
    }
}