using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.MessageQueuing.Publish;

namespace DM.Services.Community.BusinessProcesses.Activation
{
    /// <inheritdoc />
    public class ActivationService : IActivationService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly IActivationRepository repository;
        private readonly IInvokedEventPublisher publisher;

        /// <inheritdoc />
        public ActivationService(
            IDateTimeProvider dateTimeProvider,
            IActivationRepository repository,
            IInvokedEventPublisher publisher)
        {
            this.dateTimeProvider = dateTimeProvider;
            this.repository = repository;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task<Guid> Activate(Guid tokenId)
        {
            var userId = await repository.FindUserToActivate(tokenId, dateTimeProvider.Now - TimeSpan.FromDays(2));
            if (userId == default)
            {
                throw new HttpException(HttpStatusCode.Gone,
                    "Activation token is invalid! Address the technical support for further assistance");
            }

            await repository.ActivateUser(
                new UpdateBuilder<User>(userId).Field(u => u.Activated, true),
                new UpdateBuilder<Token>(tokenId).Field(t => t.IsRemoved, true));

            await publisher.Publish(EventType.ActivatedUser, userId);
            return userId;
        }
    }
}