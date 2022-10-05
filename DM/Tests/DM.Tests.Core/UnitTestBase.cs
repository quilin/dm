using System;
using Moq;

namespace DM.Tests.Core;

public abstract class UnitTestBase : IDisposable
{
    private readonly MockRepository repository;

    protected UnitTestBase()
    {
        repository = new MockRepository(MockBehavior.Loose);
    }

    protected Mock<T> Mock<T>(MockBehavior behavior = MockBehavior.Loose) where T : class =>
        repository.Create<T>(behavior);

    public virtual void Dispose() => repository.Verify();
}