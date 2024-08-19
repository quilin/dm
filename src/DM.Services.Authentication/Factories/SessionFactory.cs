using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories;

/// <inheritdoc />
internal class SessionFactory(
    IGuidFactory guidFactory,
    IDateTimeProvider dateTimeProvider) : ISessionFactory
{
    /// <inheritdoc />
    public Session Create(bool persistent, bool invisible)
    {
        var rightNow = dateTimeProvider.Now.UtcDateTime;
        return new Session
        {
            Id = guidFactory.Create(),
            Persistent = persistent,
            Invisible = invisible,
            ExpirationDate = persistent
                ? rightNow.AddMonths(1)
                : rightNow.AddDays(1)
        };
    }
}