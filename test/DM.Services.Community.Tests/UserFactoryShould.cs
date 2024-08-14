using System;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Tests.Core;
using FluentAssertions;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests;

public class UserFactoryShould : UnitTestBase
{
    private readonly ISetup<IGuidFactory, Guid> idCreateSetup;
    private readonly ISetup<IDateTimeProvider, DateTimeOffset> currentMomentSetup;
    private readonly UserFactory factory;

    public UserFactoryShould()
    {
        var guidFactory = Mock<IGuidFactory>();
        idCreateSetup = guidFactory.Setup(f => f.Create());
        var dateTimeProvider = Mock<IDateTimeProvider>();
        currentMomentSetup = dateTimeProvider.Setup(p => p.Now);
        factory = new UserFactory(guidFactory.Object, dateTimeProvider.Object);
    }

    [Fact]
    public void CreateUserWithNewIdAndCurrentMomentRegistrationDate()
    {
        var userId = Guid.NewGuid();
        idCreateSetup.Returns(userId);
        var rightNow = new DateTime(2018, 6, 11);
        currentMomentSetup.Returns(rightNow);

        var actual = factory.Create(new UserRegistration
        {
            Email = "email",
            Login = " some login  ",
            Password = "who cares"
        }, "salt", "hash");
        actual.Should().BeEquivalentTo(new User
        {
            UserId = userId,
            Login = "some login",
            Email = "email",
            RegistrationDate = rightNow,
            LastVisitDate = null,
            Role = UserRole.Player,
            AccessPolicy = AccessPolicy.NotSpecified,
            Salt = "salt",
            PasswordHash = "hash",
            RatingDisabled = false,
            QualityRating = 0,
            QuantityRating = 0,
            Activated = false,
            CanMerge = false,
            MergeRequested = null,
            IsRemoved = false
        });
    }
}