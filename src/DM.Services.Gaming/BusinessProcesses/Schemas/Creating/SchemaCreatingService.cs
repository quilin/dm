using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <inheritdoc />
internal class SchemaCreatingService : ISchemaCreatingService
{
    private readonly ISchemaCreatingValidator validator;
    private readonly IIntentionManager intentionManager;
    private readonly ISchemaFactory factory;
    private readonly ISchemaCreatingRepository repository;
    private readonly IIdentityProvider identityProvider;

    /// <inheritdoc />
    public SchemaCreatingService(
        ISchemaCreatingValidator validator,
        IIntentionManager intentionManager,
        ISchemaFactory factory,
        ISchemaCreatingRepository repository,
        IIdentityProvider identityProvider)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.factory = factory;
        this.repository = repository;
        this.identityProvider = identityProvider;
    }

    /// <inheritdoc />
    public async Task<AttributeSchema> Create(AttributeSchema attributeSchema)
    {
        validator.ValidateAndThrow(attributeSchema);
        intentionManager.ThrowIfForbidden(GameIntention.Create);

        var schemaToCreate = factory.CreateNew(attributeSchema, identityProvider.Current.User.UserId);
        return await repository.Create(schemaToCreate);
    }
}