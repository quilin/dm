using System;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses;

public class UserFactoryShould : UnitTestBase
{
    private readonly ISetup<IGuidFactory, Guid> newIdSetup;
    private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMomentSetup;
    private readonly UserFactory factory;

    public UserFactoryShould()
    {
        var guidFactory = Mock<IGuidFactory>();
        newIdSetup = guidFactory.Setup(f => f.Create());
        var dateTimeProvider = Mock<IDateTimeProvider>();
        currentMomentSetup = dateTimeProvider.Setup(p => p.Now);
        factory = new UserFactory(guidFactory.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void CreateNewUser()
    {
        var userId = Guid.NewGuid();
        newIdSetup.Returns(userId);
        var rightNow = new DateTimeOffset(2019, 05, 12, 11, 07, 10, TimeSpan.Zero);
        currentMomentSetup.Returns(rightNow);
        var actual = factory.Create(new UserRegistration
        {
            Email = "email  ",
            Login = "   login",
            Password = "whatever"
        }, "salt", "hash");

        actual.Should().BeEquivalentTo(new User
        {
            UserId = userId,
            Email = "email",
            Login = "login",
            Salt = "salt",
            PasswordHash = "hash",
            Activated = false,
            LastVisitDate = null,
            Role = UserRole.Player,
            QualityRating = 0,
            QuantityRating = 0,
            RatingDisabled = false,
            CanMerge = false,
            MergeRequested = null,
            IsRemoved = false,
            RegistrationDate = rightNow
        });
    }
}