using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <inheritdoc />
internal class SchemaCreatingService(
    ISchemaCreatingValidator validator,
    IIntentionManager intentionManager,
    ISchemaFactory factory,
    ISchemaCreatingRepository repository,
    IIdentityProvider identityProvider) : ISchemaCreatingService
{
    /// <inheritdoc />
    public async Task<AttributeSchema> Create(AttributeSchema attributeSchema, CancellationToken cancellationToken)
    {
        validator.ValidateAndThrow(attributeSchema);
        intentionManager.ThrowIfForbidden(GameIntention.Create);

        var schemaToCreate = factory.CreateNew(attributeSchema, identityProvider.Current.User.UserId);
        return await repository.Create(schemaToCreate, cancellationToken);
    }
}