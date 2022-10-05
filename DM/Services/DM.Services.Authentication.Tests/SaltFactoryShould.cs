using DM.Services.Authentication.Implementation.Security;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests;

public class SaltFactoryShould
{
    private readonly SaltFactory saltFactory;

    public SaltFactoryShould()
    {
        saltFactory = new SaltFactory();
    }

    [Fact]
    public void GenerateSaltOfLimitedLength()
    {
        saltFactory.Create(20).Length.Should().BeLessOrEqualTo(20);
    }
}