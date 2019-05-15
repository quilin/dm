using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories
{
    /// <inheritdoc />
    public class SessionFactory : ISessionFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public SessionFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc />
        public Session Create(bool persistent)
        {
            return new Session
            {
                Id = guidFactory.Create(),
                IsPersistent = persistent,
                ExpirationDate = dateTimeProvider.Later(TimeSpan.FromDays(persistent ? 30 : 1))
            };
        }
    }
}