using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Schemas.Creating;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating
{
    /// <inheritdoc />
    public class SchemaUpdatingService : ISchemaUpdatingService
    {
        private readonly ISchemaCreatingValidator validator;
        private readonly ISchemaReadingService readingService;
        private readonly IIntentionManager intentionManager;
        private readonly ISchemaFactory schemaFactory;
        private readonly ISchemaUpdatingRepository repository;
        private readonly IMapper mapper;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaUpdatingService(
            ISchemaCreatingValidator validator,
            ISchemaReadingService readingService,
            IIntentionManager intentionManager,
            ISchemaFactory schemaFactory,
            ISchemaUpdatingRepository repository,
            IMapper mapper,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.readingService = readingService;
            this.intentionManager = intentionManager;
            this.schemaFactory = schemaFactory;
            this.repository = repository;
            this.mapper = mapper;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<AttributeSchema> Update(AttributeSchema attributeSchema)
        {
            validator.ValidateAndThrow(attributeSchema);
            var oldSchema = await readingService.Get(attributeSchema.Id);
            intentionManager.ThrowIfForbidden(AttributeSchemaIntention.Edit, oldSchema);

            var schemaToUpdate = schemaFactory.CreateToUpdate(attributeSchema, identity.User.UserId);
            var updatedSchema = await repository.UpdateSchema(schemaToUpdate);
            return mapper.Map<AttributeSchema>(updatedSchema);
        }
    }
}