using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Shared;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <inheritdoc />
    public class SchemaCreatingService : ISchemaCreatingService
    {
        private readonly ISchemaCreatingValidator validator;
        private readonly IIntentionManager intentionManager;
        private readonly ISchemaFactory factory;
        private readonly ISchemaCreatingRepository repository;
        private readonly IMapper mapper;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaCreatingService(
            ISchemaCreatingValidator validator,
            IIntentionManager intentionManager,
            ISchemaFactory factory,
            ISchemaCreatingRepository repository,
            IIdentityProvider identityProvider,
            IMapper mapper)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.factory = factory;
            this.repository = repository;
            this.mapper = mapper;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Create(AttributeSchema attributeSchema)
        {
            validator.ValidateAndThrow(attributeSchema);
            intentionManager.ThrowIfForbidden(GameIntention.Create);

            var schemaToCreate = factory.CreateNew(attributeSchema, identity.User.UserId);
            var createdSchema = await repository.Create(schemaToCreate);
            return mapper.Map<AttributeSchema>(createdSchema);
        }
    }
}