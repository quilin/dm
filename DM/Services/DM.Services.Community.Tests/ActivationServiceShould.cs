using System;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Activation;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Exceptions;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests
{
    public class ActivationServiceShould : UnitTestBase
    {
        private readonly ActivationService activationService;
        private readonly ISetup<IActivationRepository, Task<Guid>> findUserSetup;
        private readonly Mock<IActivationRepository> activationRepository;
        private readonly Mock<IInvokedEventPublisher> publisher;

        public ActivationServiceShould()
        {
            activationRepository = Mock<IActivationRepository>();
            findUserSetup = activationRepository
                .Setup(r => r.FindUserToActivate(It.IsAny<Guid>(), It.IsAny<DateTimeOffset>()));
            activationRepository
                .Setup(r => r.ActivateUser(It.IsAny<UpdateBuilder<User>>(), It.IsAny<UpdateBuilder<Token>>()))
                .Returns(Task.CompletedTask);

            var dateTimeProvider = Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(p => p.Now).Returns(new DateTime(2019, 01, 02));

            publisher = Mock<IInvokedEventPublisher>();
            publisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            activationService = new ActivationService(dateTimeProvider.Object,
                activationRepository.Object, publisher.Object);
        }

        [Fact]
        public async Task SearchLastTwoDaysTokens()
        {
            var tokenId = Guid.NewGuid();
            findUserSetup.ReturnsAsync(Guid.NewGuid());
            await activationService.Activate(tokenId);
            activationRepository.Verify(r => r.FindUserToActivate(tokenId, new DateTime(2018, 12, 31)), Times.Once);
        }

        [Fact]
        public void ThrowGoneException_WhenTokenInvalid()
        {
            findUserSetup.ReturnsAsync(Guid.Empty);
            activationService
                .Invoking(s => s.Activate(Guid.NewGuid()).GetAwaiter().GetResult())
                .Should().Throw<HttpException>()
                .And.StatusCode.Should().Be(HttpStatusCode.Gone);
        }

        [Fact]
        public async Task ReturnActivatedUserId_WhenTokenValid()
        {
            var tokenId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            findUserSetup.ReturnsAsync(userId);

            var actual = await activationService.Activate(tokenId);
            actual.Should().Be(userId);
        }

        [Fact]
        public async Task ActivateFoundUser_WhenTokenValid()
        {
            var tokenId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            findUserSetup.ReturnsAsync(userId);

            await activationService.Activate(tokenId);

            // TODO: Get to verify the updating operations
//            activationRepository.Verify(r => r.ActivateUser(
//                It.Is<UpdateBuilder<User>>(x => x.Equals(new UpdateBuilder<User>(userId)
//                    .Field(u => u.Activated, true))),
//                It.Is<UpdateBuilder<Token>>(x => x.Equals(new UpdateBuilder<Token>(tokenId)
//                    .Field(t => t.IsRemoved, true)))), Times.Once);
        }

        [Fact]
        public async Task PublishActivationMessage_WhenTokenValid()
        {
            var tokenId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            findUserSetup.ReturnsAsync(userId);

            await activationService.Activate(tokenId);

            publisher.Verify(p => p.Publish(EventType.ActivatedUser, userId), Times.Once);
        }
    }
}