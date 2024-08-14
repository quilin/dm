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
internal class SchemaUpdatingService : ISchemaUpdatingService
{
    private readonly ISchemaCreatingValidator validator;
    private readonly ISchemaReadingService readingService;
    private readonly IIntentionManager intentionManager;
    private readonly ISchemaFactory schemaFactory;
    private readonly ISchemaUpdatingRepository repository;
    private readonly IMapper mapper;
    private readonly IIdentityProvider identityProvider;

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
        this.identityProvider = identityProvider;
    }
        
    /// <inheritdoc />
    public async Task<AttributeSchema> Update(AttributeSchema attributeSchema)
    {
        validator.ValidateAndThrow(attributeSchema);
        var oldSchema = await readingService.Get(attributeSchema.Id);
        intentionManager.ThrowIfForbidden(AttributeSchemaIntention.Edit, oldSchema);
            
        var schemaToUpdate = schemaFactory.CreateToUpdate(attributeSchema, identityProvider.Current.User.UserId);
        var updatedSchema = await repository.UpdateSchema(schemaToUpdate);
        return mapper.Map<AttributeSchema>(updatedSchema);
    }
}