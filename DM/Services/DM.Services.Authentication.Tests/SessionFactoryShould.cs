using System;
using DM.Services.Authentication.Factories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class SessionFactoryShould
{
    private readonly ISetup<IGuidFactory, Guid> createIdSetup;
    private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMoment;
    private readonly SessionFactory sessionFactory;

    public SessionFactoryShould()
    {
        var guidFactory = new Mock<IGuidFactory>();
        createIdSetup = guidFactory.Setup(f => f.Create());
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        currentMoment = dateTimeProvider.Setup(p => p.Now);
        sessionFactory = new SessionFactory(guidFactory.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void CreatePersistentSession()
    {
        var sessionId = Guid.NewGuid();
        createIdSetup.Returns(sessionId);
        currentMoment.Returns(new DateTimeOffset(2017, 12, 5, 0, 0, 0, TimeSpan.Zero));

        var actual = sessionFactory.Create(true, false);
        actual.Should().BeEquivalentTo(new Session
        {
            Id = sessionId,
            Persistent = true,
            Invisible = false,
            ExpirationDate = new DateTime(2018, 1, 5)
        });
    }

    [Fact]
    public void CreateNonPersistentSession()
    {
        var sessionId = Guid.NewGuid();
        createIdSetup.Returns(sessionId);
        currentMoment.Returns(new DateTimeOffset(2017, 7, 13, 0, 0, 0, TimeSpan.Zero));

        var actual = sessionFactory.Create(false, true);
        actual.Should().BeEquivalentTo(new Session
        {
            Id = sessionId,
            Persistent = false,
            Invisible = true,
            ExpirationDate = new DateTime(2017, 7, 14)
        });
    }
}