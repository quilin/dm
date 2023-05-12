using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;
using DM.Services.Forum.BusinessProcesses.Common;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Tests.Dsl;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Fora;

public class ForumReadingServiceShould
{
    private readonly ISetup<IIdentity, AuthenticatedUser> userSetup;
    private readonly ISetup<IAccessPolicyConverter, ForumAccessPolicy> accessPolicySetup;
    private readonly ISetup<IForumRepository, Task<IEnumerable<Dto.Output.Forum>>> getForaSetup;
    private readonly ISetup<IUnreadCountersRepository, Task<IDictionary<Guid, int>>> getCountersSetup;
    private readonly ForumReadingService service;

    public ForumReadingServiceShould()
    {
        var identityProvider = new Mock<IIdentityProvider>();
        var identity = new Mock<IIdentity>();
        userSetup = identity.Setup(i => i.User);
        identityProvider.Setup(p => p.Current).Returns(identity.Object);

        var accessPolicyConverter = new Mock<IAccessPolicyConverter>();
        accessPolicySetup = accessPolicyConverter.Setup(c => c.Convert(It.IsAny<UserRole>()));

        var repository = new Mock<IForumRepository>();
        getForaSetup = repository.Setup(r => r.SelectFora(It.IsAny<ForumAccessPolicy?>()));

        var unreadCounters = new Mock<IUnreadCountersRepository>();
        getCountersSetup = unreadCounters
            .Setup(c => c.SelectByParents(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<Guid[]>()));

        service = new ForumReadingService(
            identityProvider.Object,
            accessPolicyConverter.Object,
            repository.Object,
            unreadCounters.Object);
    }

    [Fact]
    public async Task ReturnForaList_AvailableForCurrentUser()
    {
        var expected = new Dto.Output.Forum[]
        {
            new() { Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2") },
            new() { Id = Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107") }
        };
        var userId = Guid.NewGuid();
        userSetup.Returns(Create.User(userId)
            .WithRole(UserRole.NannyModerator)
            .Please());
        accessPolicySetup.Returns(ForumAccessPolicy.Player | ForumAccessPolicy.ForumModerator);
        getForaSetup.ReturnsAsync(expected);
        getCountersSetup.ReturnsAsync(new Dictionary<Guid, int>
        {
            [Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2")] = 5,
            [Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107")] = 2,
        });

        var actual = await service.GetForaList();
        actual.Should().BeEquivalentTo(new Dto.Output.Forum[]
        {
            new() { Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2"), UnreadTopicsCount = 5 },
            new() { Id = Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107"), UnreadTopicsCount = 2 }
        });
    }

    [Fact]
    public async Task ReturnMatchingForum_WhenAvailable()
    {
        var expected = new Dto.Output.Forum[]
        {
            new() { Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2"), Title = "Test" },
            new() { Id = Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107"), Title = "Whatever" }
        };
        var userId = Guid.NewGuid();
        userSetup.Returns(Create.User(userId)
            .WithRole(UserRole.NannyModerator)
            .Please());
        accessPolicySetup.Returns(ForumAccessPolicy.Player | ForumAccessPolicy.ForumModerator);
        getForaSetup.ReturnsAsync(expected);

        var actual = await service.GetForum("Test");
        actual.Should().BeEquivalentTo(expected[0]);
    }

    [Fact]
    public async Task Throw_WhenNotAvailable()
    {
        var expected = new Dto.Output.Forum[]
        {
            new() { Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2"), Title = "Test" },
            new() { Id = Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107"), Title = "Whatever" }
        };
        var userId = Guid.NewGuid();
        userSetup.Returns(Create.User(userId)
            .WithRole(UserRole.NannyModerator)
            .Please());
        accessPolicySetup.Returns(ForumAccessPolicy.Player | ForumAccessPolicy.ForumModerator);
        getForaSetup.ReturnsAsync(expected);

        (await service.Invoking(s => s.GetForum("Something"))
            .Should().ThrowAsync<HttpException>()).And
            .StatusCode.Should().Be(HttpStatusCode.Gone);
    }

    [Fact]
    public async Task ReturnMatchingForum_EnrichedWithCounters()
    {
        var expected = new Dto.Output.Forum[]
        {
            new() { Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2"), Title = "Test" },
            new() { Id = Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107"), Title = "Whatever" }
        };
        var userId = Guid.NewGuid();
        userSetup.Returns(Create.User(userId)
            .WithRole(UserRole.NannyModerator)
            .Please());
        accessPolicySetup.Returns(ForumAccessPolicy.Player | ForumAccessPolicy.ForumModerator);
        getForaSetup.ReturnsAsync(expected);
        getCountersSetup.ReturnsAsync(new Dictionary<Guid, int>
        {
            [Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2")] = 5,
            [Guid.Parse("6647C4DB-BC1B-4F65-87EF-88FA5A6C0107")] = 2,
        });

        var actual = await service.GetSingleForum("Test");
        actual.Should().BeEquivalentTo(new Dto.Output.Forum
        {
            Id = Guid.Parse("8EED1383-9964-44F4-AF07-4DB6843BADF2"), UnreadTopicsCount = 5, Title = "Test"
        });
    }
}