using System;
using System.Linq.Expressions;
using DM.Services.DataAccess.RelationalStorage;
using Moq;

namespace DM.Tests.Core
{
    public abstract class UnitTestBase : IDisposable
    {
        private readonly MockRepository repository;

        protected UnitTestBase()
        {
            repository = new MockRepository(MockBehavior.Loose);
        }

        protected Mock<T> Mock<T>(MockBehavior behavior = MockBehavior.Loose) where T : class =>
            repository.Create<T>(behavior);

        protected Mock<IUpdateBuilder<TEntity>> MockUpdateBuilder<TEntity>(MockBehavior behavior = MockBehavior.Loose)
            where TEntity : class, new()
        {
            var mock = Mock<IUpdateBuilder<TEntity>>(behavior);
            mock.Setup(m => m.Field(It.IsAny<Expression<Func<TEntity, object>>>(), It.IsAny<object>()))
                .Returns(mock.Object);
            return mock;
        }

        public virtual void Dispose() => repository.Verify();
    }
}