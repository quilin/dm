using System;
using System.Text;
using DM.Services.Authentication.Implementation.Security;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class SecurityManagerShould : UnitTestBase
{
    private readonly Mock<ISaltFactory> saltFactory;
    private readonly Mock<IHashProvider> hashProvider;
    private readonly ISetup<IHashProvider, byte[]> computeHashSetup;
    private readonly SecurityManager securityManager;

    public SecurityManagerShould()
    {
        saltFactory = Mock<ISaltFactory>();

        hashProvider = Mock<IHashProvider>();
        computeHashSetup = hashProvider.Setup(p => p.ComputeSha256(It.IsAny<string>(), It.IsAny<string>()));

        securityManager = new SecurityManager(saltFactory.Object, hashProvider.Object);
    }

    [Fact]
    public void EncryptPasswordForStorage()
    {
        saltFactory.Setup(f => f.Create(It.IsAny<int>())).Returns("salt");
        var expectedHash = Convert.ToBase64String(Encoding.UTF8.GetBytes("hash"));
        var expectedHashBytes = Convert.FromBase64String(expectedHash);
        computeHashSetup.Returns(expectedHashBytes);

        var (actualHash, actualSalt) = securityManager.GeneratePassword("qwerty");
        actualHash.Should().Be(expectedHash);
        actualSalt.Should().Be("salt");
        hashProvider.Verify(p => p.ComputeSha256("qwerty", "salt"), Times.Once);
        hashProvider.VerifyNoOtherCalls();
    }

    [Fact]
    public void ConfirmPasswordEquivalency()
    {
        var base64Hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("hash"));
        computeHashSetup.Returns(Encoding.UTF8.GetBytes("hash"));

        securityManager.ComparePasswords("qwerty", "salt", base64Hash).Should().BeTrue();
        hashProvider.Verify(p => p.ComputeSha256("qwerty", "salt"), Times.Once);
        hashProvider.VerifyNoOtherCalls();
    }

    [Fact]
    public void ConfirmPasswordInequality()
    {
        var base64Hash = Convert.ToBase64String(Encoding.UTF8.GetBytes("hash"));
        computeHashSetup.Returns(Encoding.UTF8.GetBytes("notHash"));

        securityManager.ComparePasswords("qwerty", "salt", base64Hash).Should().BeFalse();
        hashProvider.Verify(p => p.ComputeSha256("qwerty", "salt"), Times.Once);
        hashProvider.VerifyNoOtherCalls();
    }
}