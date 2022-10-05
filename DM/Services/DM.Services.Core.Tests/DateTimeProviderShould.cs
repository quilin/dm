using System;
using DM.Services.Core.Implementation;
using FluentAssertions;
using Xunit;

namespace DM.Services.Core.Tests;

public class DateTimeProviderShould
{
    [Fact]
    public void ProvideCurrentMomentUtcDate()
    {
        var actual = new DateTimeProvider().Now;
        actual.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}