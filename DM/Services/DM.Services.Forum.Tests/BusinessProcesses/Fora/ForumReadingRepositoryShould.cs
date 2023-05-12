using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Core.Dto.Enums;
using DM.Services.Forum.BusinessProcesses.Fora;
using DM.Services.Forum.Dto;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Fora;

[Collection(nameof(DbCollection))]
public class ForumReadingRepositoryShould
{
    private readonly DbFixture dbFixture;

    public ForumReadingRepositoryShould(DbFixture dbFixture)
    {
        this.dbFixture = dbFixture;
    }

    [Fact]
    public async Task ContainPrePopulatedFora()
    {
        
        var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ForumProfile>()));
        var repository = new ForumRepository(
            (await dbFixture.CreateDbs()).Rdb,
            new MemoryCache(new MemoryCacheOptions()),
            mapper);

        var policy = Enum.GetValues<ForumAccessPolicy>()
            .Aggregate(ForumAccessPolicy.NoOne, (seed, policy) => seed | policy);
        var actual = await repository.SelectFora(policy);

        actual.Should().NotBeEmpty();
    }
}