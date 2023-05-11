using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DM.Services.Common.Tests;

public class IntentionManagerShould
{
    private readonly IntentionManager manager;
    private readonly Mock<IIntentionResolver<Intention, Target1>> resolver1;
    private readonly Mock<IIntentionResolver<Intention, Target2>> resolver2;
    private readonly Mock<IIntentionResolver<Intention>> simpleResolver;
    private readonly Mock<IIdentity> identity;

    public IntentionManagerShould()
    {
        resolver1 = new Mock<IIntentionResolver<Intention, Target1>>();
        resolver2 = new Mock<IIntentionResolver<Intention, Target2>>();
        simpleResolver = new Mock<IIntentionResolver<Intention>>();
        var resolvers = new IIntentionResolver[] { resolver1.Object, resolver2.Object, simpleResolver.Object };

        var identityProvider = new Mock<IIdentityProvider>();
        identity = new Mock<IIdentity>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        manager = new IntentionManager(identityProvider.Object, resolvers,
            new Mock<ILogger<IntentionManager>>().Object);
    }

    [Fact]
    public void ReturnFalse_WhenNoMatchingResolvers()
    {
        manager.IsAllowed(Intention.Value, new Target3()).Should().BeFalse();
        manager.IsAllowed(UserRole.Administrator).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WhenMatchingResolverResolvesFalse()
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
    public void ReturnFalse_WhenMatchingSimpleResolverResolvesFalse()
    {
        simpleResolver
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>()))
            .Returns(false);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var actual = manager.IsAllowed(Intention.Value);
        actual.Should().BeFalse();
        
        simpleResolver.Verify(r => r.IsAllowed(user, Intention.Value), Times.Once);
        simpleResolver.VerifyNoOtherCalls();
    }

    [Fact]
    public void ThrowException_WhenMatchingResolverResolvesFalse()
    {
        resolver1
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>(), It.IsAny<Target1>()))
            .Returns(false);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var target = new Target1();
        manager.Invoking(m => m.ThrowIfForbidden(Intention.Value, target))
            .Should().Throw<IntentionManagerException>();

        resolver1.Verify(r => r.IsAllowed(user, Intention.Value, target), Times.Once);
        resolver1.VerifyNoOtherCalls();
    }

    [Fact]
    public void ThrowException_WhenMatchingSimpleResolverResolvesFalse()
    {
        simpleResolver
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>()))
            .Returns(false);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        manager.Invoking(m => m.ThrowIfForbidden(Intention.Value))
            .Should().Throw<IntentionManagerException>();

        simpleResolver.Verify(r => r.IsAllowed(user, Intention.Value), Times.Once);
        simpleResolver.VerifyNoOtherCalls();
    }

    [Fact]
    public void ReturnTrue_WhenMatchingResolverResolvesTrue()
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
    public void ReturnTrue_WhenMatchingSimpleResolverResolvesTrue()
    {
        simpleResolver
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>()))
            .Returns(true);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        var actual = manager.IsAllowed(Intention.Value);
        actual.Should().BeTrue();
        simpleResolver.Verify(r => r.IsAllowed(user, Intention.Value), Times.Once);
        simpleResolver.VerifyNoOtherCalls();
    }

    [Fact]
    public void NotThrowException_WhenMatchingResolverResolvesTrue()
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

    [Fact]
    public void NotThrowException_WhenMatchingSimpleResolverResolvesTrue()
    {
        simpleResolver
            .Setup(r => r.IsAllowed(It.IsAny<AuthenticatedUser>(), It.IsAny<Intention>()))
            .Returns(true);
        var user = new AuthenticatedUser();
        identity.Setup(i => i.User).Returns(user);

        manager.Invoking(m => m.ThrowIfForbidden(Intention.Value))
            .Should()
            .NotThrow<IntentionManagerException>();

        simpleResolver.Verify(r => r.IsAllowed(user, Intention.Value), Times.Once);
        simpleResolver.VerifyNoOtherCalls();
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