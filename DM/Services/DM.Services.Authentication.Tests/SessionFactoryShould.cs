using System;
using DM.Services.Authentication.Factories;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Testing.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Authentication.Tests
{
    public class SessionFactoryShould : UnitTestBase
    {
        private readonly Mock<IGuidFactory> guidFactory;
        private readonly Mock<IDateTimeProvider> dateTimeProvider;
        private readonly SessionFactory sessionFactory;

        public SessionFactoryShould()
        {
            guidFactory = Mock<IGuidFactory>();
            dateTimeProvider = Mock<IDateTimeProvider>();
            sessionFactory = new SessionFactory(guidFactory.Object, dateTimeProvider.Object);
        }

        [Fact]
        public void CreateLongSessionForPersistent()
        {
            var sessionId = Guid.NewGuid();
            guidFactory.Setup(f => f.Create()).Returns(sessionId);
            var currentMoment = new DateTime(2017, 5, 17);
            dateTimeProvider.Setup(p => p.Now).Returns(currentMoment);
            
            var actual = sessionFactory.Create(true);
            actual.Should().BeEquivalentTo(new Session
            {
                Id = sessionId,
                IsPersistent = true,
                ExpirationDate = new DateTime(2017, 6, 17)
            });
        }

        [Fact]
        public void CreateShortSessionForNonPersistent()
        {
            var sessionId = Guid.NewGuid();
            guidFactory.Setup(f => f.Create()).Returns(sessionId);
            var currentMoment = new DateTime(2017, 4, 12);
            dateTimeProvider.Setup(p => p.Now).Returns(currentMoment);
            
            var actual = sessionFactory.Create(false);
            actual.Should().BeEquivalentTo(new Session
            {
                Id = sessionId,
                IsPersistent = false,
                ExpirationDate = new DateTime(2017, 4, 13)
            });
        }
    }
}