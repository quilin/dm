using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.MessageQueuing.GeneralBus;

namespace DM.Services.Community.BusinessProcesses.Account.Activation;

/// <inheritdoc />
internal class ActivationService : IActivationService
{
    private readonly IDateTimeProvider dateTimeProvider;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IActivationRepository repository;
    private readonly IInvokedEventProducer producer;

    /// <inheritdoc />
    public ActivationService(
        IDateTimeProvider dateTimeProvider,
        IUpdateBuilderFactory updateBuilderFactory,
        IActivationRepository repository,
        IInvokedEventProducer producer)
    {
        this.dateTimeProvider = dateTimeProvider;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
        this.producer = producer;
    }
        
    /// <inheritdoc />
    public async Task<Guid> Activate(Guid tokenId)
    {
        var userId = await repository.FindUserToActivate(tokenId, dateTimeProvider.Now - TimeSpan.FromDays(2));
        if (!userId.HasValue)
        {
            throw new HttpException(HttpStatusCode.Gone,
                "Activation token is invalid! Address the technical support for further assistance");
        }

        var updateUser = updateBuilderFactory.Create<User>(userId.Value).Field(u => u.Activated, true);
        var updateToken = updateBuilderFactory.Create<Token>(tokenId).Field(t => t.IsRemoved, true);
        await repository.ActivateUser(updateUser, updateToken);

        await producer.Send(EventType.ActivatedUser, userId.Value);
        return userId.Value;
    }
}