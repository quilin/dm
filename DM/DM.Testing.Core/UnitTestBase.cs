using System;
using Moq;

namespace DM.Testing.Core
{
    public abstract class UnitTestBase : IDisposable
    {
        private readonly MockRepository repository;

        protected UnitTestBase()
        {
            repository = new MockRepository(MockBehavior.Strict);
        }

        protected Mock<T> Mock<T>(MockBehavior behavior = MockBehavior.Strict) where T : class =>
            repository.Create<T>(behavior);

        public virtual void Dispose()
        {
            repository.VerifyAll();
        }
    }
}