using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using FluentValidation;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating
{
    /// <inheritdoc />
    public class SchemaCreatingService : ISchemaCreatingService
    {
        private readonly IValidator<CreateAttributeSchema> validator;
        private readonly IIntentionManager intentionManager;
        private readonly ISchemaFactory schemaFactory;
        private readonly ISchemaCreatingRepository repository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaCreatingService(
            IValidator<CreateAttributeSchema> validator,
            IIntentionManager intentionManager,
            ISchemaFactory schemaFactory,
            ISchemaCreatingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.validator = validator;
            this.intentionManager = intentionManager;
            this.schemaFactory = schemaFactory;
            this.repository = repository;
            identity = identityProvider.Current;
        }
        
        /// <inheritdoc />
        public async Task<AttributeSchema> Create(CreateAttributeSchema createAttributeSchema)
        {
            await validator.ValidateAndThrowAsync(createAttributeSchema);
            intentionManager.ThrowIfForbidden(GameIntention.Create);

            var attributeSchema = schemaFactory.Create(createAttributeSchema, identity.User.UserId);
            return await repository.Create(attributeSchema);
        }
    }
}