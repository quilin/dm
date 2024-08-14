using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Tests.Core;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DM.Services.Common.Tests;

public class IntentionManagerShould : UnitTestBase
{
    private readonly IntentionManager manager;
    private readonly Mock<IIntentionResolver<Intention, Target1>> resolver1;
    private readonly Mock<IIntentionResolver<Intention, Target2>> resolver2;
    private readonly Mock<IIdentity> identity;

    public IntentionManagerShould()
    {
        resolver1 = Mock<IIntentionResolver<Intention, Target1>>();
        resolver2 = Mock<IIntentionResolver<Intention, Target2>>();
        var resolvers = new IIntentionResolver[] {resolver1.Object, resolver2.Object};

        var identityProvider = Mock<IIdentityProvider>();
        identity = Mock<IIdentity>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        manager = new IntentionManager(identityProvider.Object, resolvers,
            Mock<ILogger<IntentionManager>>().Object);
    }

    [Fact]
    public void ReturnFalseIfNoMatchingResolvers()
    {
        var actual = manager.IsAllowed(Intention.Value, new Target3());
        actual.Should().BeFalse();
    }

    [Fact]
    public void ReturnFalseIfMatchingResolverResolvesFalse()
    {
        resolver1
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>(), It.IsAny<Target1>()))
            .Returns(false);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var target = new Target1();
        var actual = manager.IsAllowed(Intention.Value, target);
        actual.Should().BeFalse();
        resolver1.Verify(r => r.IsAllowed(user, Intention.Value, target), Times.Once);
        resolver1.VerifyNoOtherCalls();
    }

    [Fact]
    public void ThrowExceptionIfMatchingResolverResolvesFalse()
    {
        resolver1
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>(), It.IsAny<Target1>()))
            .Returns(false);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var target = new Target1();
        manager.Invoking(m => m.ThrowIfForbidden(Intention.Value, target))
            .Should()
            .Throw<IntentionManagerException>();

        resolver1.Verify(r => r.IsAllowed(user, Intention.Value, target), Times.Once);
        resolver1.VerifyNoOtherCalls();
    }

    [Fact]
    public void ReturnTrueIfMatchingResolverResolvesTrue()
    {
        resolver2
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>(), It.IsAny<Target2>()))
            .Returns(true);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var target = new Target2();
        var actual = manager.IsAllowed(Intention.Value, target);
        actual.Should().BeTrue();
        resolver2.Verify(r => r.IsAllowed(user, Intention.Value, target), Times.Once);
        resolver2.VerifyNoOtherCalls();
    }

    [Fact]
    public void NotThrowExceptionIfMatchingResolverResolvesTrue()
    {
        resolver2
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>(), It.IsAny<Target2>()))
            .Returns(true);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var target = new Target2();
        manager.Invoking(m => m.ThrowIfForbidden(Intention.Value, target))
            .Should()
            .NotThrow<IntentionManagerException>();

        resolver2.Verify(r => r.IsAllowed(user, Intention.Value, target), Times.Once);
        resolver2.VerifyNoOtherCalls();
    }

    public enum Intention
    {
        Value
    }

    public class Target1
    {
    }

    public class Target2
    {
    }

    public class Target3
    {
    }
}