using System;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses;

public class RegistrationTokenFactoryShould : UnitTestBase
{
    private readonly ISetup<IGuidFactory, Guid> newIdSetup;
    private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMomentSetup;
    private readonly ActivationTokenFactory factory;

    public RegistrationTokenFactoryShould()
    {
        var guidFactory = Mock<IGuidFactory>();
        newIdSetup = guidFactory.Setup(f => f.Create());

        var dateTimeProvider = Mock<IDateTimeProvider>();
        currentMomentSetup = dateTimeProvider.Setup(p => p.Now);

        factory = new ActivationTokenFactory(guidFactory.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void CreateNewRegistrationToken()
    {
        var dateTimeOffset = new DateTimeOffset(2019, 05, 02, 16, 10, 10, TimeSpan.Zero);
        currentMomentSetup.Returns(dateTimeOffset);
        var tokenId = Guid.NewGuid();
        newIdSetup.Returns(tokenId);

        var userId = Guid.NewGuid();
        var actual = factory.Create(userId);

        actual.Should().BeEquivalentTo(new Token
        {
            TokenId = tokenId,
            UserId = userId,
            Type = TokenType.Activation,
            CreateDate = dateTimeOffset,
            IsRemoved = false
        });
    }
}