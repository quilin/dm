using DM.Services.Core.Implementation;
using FluentAssertions;
using Xunit;

namespace DM.Services.Core.Tests;

public class GuidFactoryShould
{
    private readonly GuidFactory guidFactory = new();

    [Fact]
    public void GenerateDifferentIds()
    {
        guidFactory.Create().Should().NotBe(guidFactory.Create());
    }

    [Fact]
    public void NotGenerateEmptyGuid()
    {
        guidFactory.Create().Should().NotBeEmpty();
    }
}