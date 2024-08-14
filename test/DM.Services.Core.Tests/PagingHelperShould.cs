using DM.Services.Core.Dto;
using DM.Tests.Core;
using FluentAssertions;
using Xunit;

namespace DM.Services.Core.Tests;

public class PagingHelperShould : UnitTestBase
{
    [Theory]
    [InlineData(1000, 10, 100)]
    [InlineData(1001, 10, 101)]
    [InlineData(999, 10, 100)]
    [InlineData(0, 10, 0)]
    public void CalculateTotalPagesCountOfGivenSize(int totalEntitiesCount, int pageSize, int expectedPagesCount)
    {
        var actual = PagingResult.Create(totalEntitiesCount, 1, pageSize);
        actual.TotalPagesCount.Should().Be(expectedPagesCount);
    }

    [Theory]
    [InlineData(1, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(25, 10, 3)]
    public void CalculateCurrentPageBasedOnGivenEntityNumberAndPageSize(int entityNumber,
        int pageSize, int expectedPageNumber)
    {
        var actual = PagingResult.Create(entityNumber + 1, entityNumber, pageSize);
        actual.CurrentPage.Should().Be(expectedPageNumber);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(-4)]
    [InlineData(5)]
    public void GuaranteeCurrentPageIsAtLeastFirst(int entityNumber)
    {
        var actual = PagingResult.Create(100, entityNumber, 10);
        actual.CurrentPage.Should().BeGreaterOrEqualTo(1);
    }
}