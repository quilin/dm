using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories
{
    public class SessionFactory : ISessionFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        public SessionFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }
        
        public Session Create(bool persistent)
        {
            return new Session
            {
                Id = guidFactory.Create(),
                IsPersistent = persistent,
                ExpirationDate = persistent
                    ? dateTimeProvider.Now.AddMonths(1)
                    : dateTimeProvider.Now.AddDays(1)
            };
        }
    }
}