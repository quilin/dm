using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Factories;
using DM.Services.Authentication.Implementation;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Authentication.Repositories;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Authentication.Tests
{
    public class RegistrationServiceShould : UnitTestBase
    {
        private readonly RegistrationService registrationService;
        private readonly ISetup<ISecurityManager, (string Hash, string Salt)> createPasswordSetup;
        private readonly ISetup<IUserFactory, User> createUserSetup;
        private readonly ISetup<IRegistrationTokenFactory, Token> createTokenSetup;
        private readonly Mock<IRegistrationRepository> registrationRepository;
        private readonly Mock<IInvokedEventPublisher> publisher;

        public RegistrationServiceShould()
        {
            var validator = Mock<IValidator<UserRegistration>>();
            validator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRegistration>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            var securityManager = Mock<ISecurityManager>();
            createPasswordSetup = securityManager.Setup(m => m
                .GeneratePassword(It.IsAny<string>()));
            var userFactory = Mock<IUserFactory>();
            createUserSetup = userFactory.Setup(f => f
                .Create(It.IsAny<UserRegistration>(), It.IsAny<string>(), It.IsAny<string>()));
            var tokenFactory = Mock<IRegistrationTokenFactory>();
            createTokenSetup = tokenFactory.Setup(f => f.Create(It.IsAny<Guid>()));

            registrationRepository = Mock<IRegistrationRepository>();
            registrationRepository
                .Setup(r => r.AddUser(It.IsAny<User>(), It.IsAny<Token>()))
                .Returns(Task.CompletedTask);
            publisher = Mock<IInvokedEventPublisher>();
            publisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);
            registrationService = new RegistrationService(validator.Object,
                securityManager.Object, userFactory.Object, tokenFactory.Object,
                registrationRepository.Object, publisher.Object, Mock<IDateTimeProvider>().Object);
        }

        [Fact]
        public async Task CreateUser()
        {
            createPasswordSetup.Returns(("hash", "salt"));
            var user = new User();
            createUserSetup.Returns(user);
            var token = new Token();
            createTokenSetup.Returns(token);

            await registrationService.Register(new UserRegistration());

            registrationRepository.Verify(r => r.AddUser(user, token), Times.Once);
            registrationRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PublishNewUserMessage()
        {
            createPasswordSetup.Returns(("hash", "salt"));
            var userId = Guid.NewGuid();
            var user = new User {UserId = userId};
            createUserSetup.Returns(user);
            var token = new Token();
            createTokenSetup.Returns(token);

            await registrationService.Register(new UserRegistration());
            
            publisher.Verify(p => p.Publish(EventType.NewUser, userId), Times.Once);
            publisher.VerifyNoOtherCalls();
        }
    }
}