using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Registration.Consumer
{
    /// <inheritdoc />
    public class RegistrationTokenFactory : IRegistrationTokenFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public RegistrationTokenFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }
        
        /// <inheritdoc />
        public Token Create(Guid userId)
        {
            return new Token
            {
                TokenId = guidFactory.Create(),
                UserId = userId,
                Type = TokenType.Registration,
                CreateDate = dateTimeProvider.Now,
                IsRemoved = false
            };
        }
    }
}