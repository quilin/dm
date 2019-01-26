using System;

namespace DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes
{
    public class AttributeSpecification
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttributeConstraints Constraints { get; set; }
    }
}