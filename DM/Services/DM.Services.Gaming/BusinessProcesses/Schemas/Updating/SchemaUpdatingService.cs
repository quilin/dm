using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Schemas.Creating;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating
{
    /// <inheritdoc />
    public class SchemaUpdatingService : ISchemaUpdatingService
    {
        private readonly IValidator<UpdateAttributeSchema> validator;
        private readonly ISchemaReadingService readingService;
        private readonly IIntentionManager intentionManager;
        private readonly ISchemaFactory schemaFactory;
        private readonly ISchemaUpdatingRepository repository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaUpdatingService(
            IValidator<UpdateAttributeSchema> validator,
            ISchemaReadingService readingService,
            IIntentionManager intentionManager,
            ISchemaFactory schemaFactory,
            ISchemaUpdatingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.readingService = readingService;
            this.intentionManager = intentionManager;
            this.schemaFactory = schemaFactory;
            this.repository = repository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<AttributeSchema> Update(UpdateAttributeSchema updateAttributeSchema)
        {
            await validator.ValidateAndThrowAsync(updateAttributeSchema);
            var oldSchema = await readingService.Get(updateAttributeSchema.SchemaId);
            intentionManager.ThrowIfForbidden(AttributeSchemaIntention.Edit, oldSchema);

            var schema = schemaFactory.Create(updateAttributeSchema, identity.User.UserId);
            return await repository.UpdateSchema(schema);
        }
    }
}