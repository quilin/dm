using System;
using DM.Services.Authentication.Implementation.Security;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class HashProviderShould
{
    private readonly SaltFactory saltFactory = new();
    private readonly HashProvider hashProvider = new();

    [Fact]
    public void GenerateHashBasedOnPlainPasswordAndValidSalt()
    {
        var validSalt = saltFactory.Create(20);
        var actual = hashProvider.ComputeSha256("password", validSalt);
        actual.Should().NotBeEmpty();
    }

    [Fact]
    public void ThrowExceptionIfSaltIsNotBase64String()
    {
        var invalidSalt = "justsomestringyboi";
        hashProvider.Invoking(p => p.ComputeSha256("password", invalidSalt))
            .Should().Throw<FormatException>();
    }
}