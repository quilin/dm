using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Gaming.Authorization;
using DM.Services.Gaming.BusinessProcesses.Schemas.Creating;
using DM.Services.Gaming.BusinessProcesses.Schemas.Reading;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Updating;

/// <inheritdoc />
internal class SchemaUpdatingService(
    ISchemaCreatingValidator validator,
    ISchemaReadingService readingService,
    IIntentionManager intentionManager,
    ISchemaFactory schemaFactory,
    ISchemaUpdatingRepository repository,
    IMapper mapper,
    IIdentityProvider identityProvider) : ISchemaUpdatingService
{
    /// <inheritdoc />
    public async Task<AttributeSchema> Update(AttributeSchema attributeSchema, CancellationToken cancellationToken)
    {
        validator.ValidateAndThrow(attributeSchema);
        var oldSchema = await readingService.Get(attributeSchema.Id, cancellationToken);
        intentionManager.ThrowIfForbidden(AttributeSchemaIntention.Edit, oldSchema);
            
        var schemaToUpdate = schemaFactory.CreateToUpdate(attributeSchema, identityProvider.Current.User.UserId);
        var updatedSchema = await repository.UpdateSchema(schemaToUpdate, cancellationToken);
        return mapper.Map<AttributeSchema>(updatedSchema);
    }
}