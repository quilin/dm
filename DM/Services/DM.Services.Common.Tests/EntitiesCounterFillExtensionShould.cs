using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Common.Extensions;
using DM.Services.DataAccess.BusinessObjects.Common;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Common.Tests;

public class EntitiesCounterFillExtensionShould
{
    [Fact]
    public async Task FillEntityCounters_WithSingleQueryCall()
    {
        var repository = new Mock<IUnreadCountersRepository>();
        var userId = Guid.NewGuid();
        var entities = new SomeEntityWithCounter[]
        {
            new() { EntityId = Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8") },
            new() { EntityId = Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF") }
        };
        repository
            .Setup(r => r.SelectByEntities(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<Guid[]>()))
            .ReturnsAsync(new Dictionary<Guid, int>
            {
                [Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8")] = 10,
                [Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF")] = 3
            });
        
        await repository.Object.FillEntityCounters(entities, userId, e => e.EntityId, e => e.Counter, UnreadEntryType.Character);

        entities.Should().BeEquivalentTo(new SomeEntityWithCounter[]
        {
            new() { EntityId = Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8"), Counter = 10 },
            new() { EntityId = Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF"), Counter = 3 }
        });
        repository.Verify(r => r.SelectByEntities(
            userId, UnreadEntryType.Character,
            Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8"), Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF")), Times.Once);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task FillParentCounters_WithSingleQueryCall()
    {
        var repository = new Mock<IUnreadCountersRepository>();
        var userId = Guid.NewGuid();
        var entities = new SomeEntityWithCounter[]
        {
            new() { EntityId = Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8") },
            new() { EntityId = Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF") }
        };
        repository
            .Setup(r => r.SelectByParents(It.IsAny<Guid>(), It.IsAny<UnreadEntryType>(), It.IsAny<Guid[]>()))
            .ReturnsAsync(new Dictionary<Guid, int>
            {
                [Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8")] = 10,
                [Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF")] = 3
            });
        
        await repository.Object.FillParentCounters(entities, userId, e => e.EntityId, e => e.Counter);

        entities.Should().BeEquivalentTo(new SomeEntityWithCounter[]
        {
            new() { EntityId = Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8"), Counter = 10 },
            new() { EntityId = Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF"), Counter = 3 }
        });
        repository.Verify(r => r.SelectByParents(
            userId, UnreadEntryType.Message,
            Guid.Parse("B0340A14-7385-4CCA-A728-87CF964C21A8"), Guid.Parse("5FA14E8E-0D8B-4F3A-9A8C-40F73374C7DF")), Times.Once);
        repository.VerifyNoOtherCalls();
    }
    
    private class SomeEntityWithCounter
    {
        public Guid EntityId { get; set; }
        public int Counter { get; set; }
    }
}