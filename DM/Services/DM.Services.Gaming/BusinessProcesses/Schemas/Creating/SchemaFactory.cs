using System;
using System.Linq;
using DM.Services.Core.Dto.Attributes;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes;
using DM.Services.Gaming.Dto.Input;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <inheritdoc />
    public class SchemaFactory : ISchemaFactory
    {
        private readonly IGuidFactory guidFactory;

        /// <inheritdoc />
        public SchemaFactory(
            IGuidFactory guidFactory)
        {
            this.guidFactory = guidFactory;
        }

        /// <inheritdoc />
        public AttributeSchema Create(CreateAttributeSchema createAttributeSchema, Guid userId)
        {
            return new AttributeSchema
            {
                Id = guidFactory.Create(),
                Name = createAttributeSchema.Name.Trim(),
                Type = SchemaType.Private,
                UserId = userId,
                Specifications = createAttributeSchema.Specifications
                    .Select(s => new AttributeSpecification
                    {
                        Id = guidFactory.Create(),
                        Name = s.Name.Trim(),
                        Constraints = s.Constraints
                    }),
                IsRemoved = false
            };
        }
    }
}