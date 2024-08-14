using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Services.Authentication.Factories;

/// <inheritdoc />
internal class SessionFactory : ISessionFactory
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