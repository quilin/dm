namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    public abstract class AttributeConstraints
    {
        public bool Required { get; set; }
        public abstract string GetDefaultValue();
    }
}