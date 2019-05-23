using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Community.BusinessProcesses.Registration;
using DM.Services.Community.BusinessProcesses.Registration.Confirmation;
using DM.Services.Community.Dto;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests
{
    public class RegistrationServiceShould : UnitTestBase
    {
        private readonly RegistrationService registrationService;
        private readonly ISetup<IUserFactory, User> createUserSetup;
        private readonly ISetup<IRegistrationTokenFactory, Token> createTokenSetup;
        private readonly Mock<IRegistrationRepository> registrationRepository;
        private readonly Mock<IRegistrationMailSender> mailSender;
        private readonly Mock<IInvokedEventPublisher> publisher;

        public RegistrationServiceShould()
        {
            var validator = Mock<IValidator<UserRegistration>>();
            validator
                .Setup(v => v.ValidateAsync(
                    It.IsAny<ValidationContext<UserRegistration>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            var securityManager = Mock<ISecurityManager>();
            securityManager.Setup(m => m
                    .GeneratePassword(It.IsAny<string>()))
                .Returns(("hash", "salt"));
            var userFactory = Mock<IUserFactory>();
            createUserSetup = userFactory.Setup(f => f
                .Create(It.IsAny<UserRegistration>(), It.IsAny<string>(), It.IsAny<string>()));
            var tokenFactory = Mock<IRegistrationTokenFactory>();
            createTokenSetup = tokenFactory.Setup(f => f.Create(It.IsAny<Guid>()));

            registrationRepository = Mock<IRegistrationRepository>();
            registrationRepository
                .Setup(r => r.AddUser(It.IsAny<User>(), It.IsAny<Token>()))
                .Returns(Task.CompletedTask);

            mailSender = Mock<IRegistrationMailSender>();
            mailSender
                .Setup(s => s.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            publisher = Mock<IInvokedEventPublisher>();
            publisher
                .Setup(p => p.Publish(It.IsAny<EventType>(), It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            registrationService = new RegistrationService(validator.Object,
                securityManager.Object, userFactory.Object, tokenFactory.Object,
                registrationRepository.Object, mailSender.Object, publisher.Object);
        }

        [Fact]
        public async Task CreateUser()
        {
            var user = new User();
            createUserSetup.Returns(user);
            createTokenSetup.Returns(new Token());

            await registrationService.Register(new UserRegistration());

            registrationRepository.Verify(r => r.AddUser(user, new Token()), Times.Once);
            registrationRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task PublishNewUserMessage()
        {
            var userId = Guid.NewGuid();
            createUserSetup.Returns(new User {UserId = userId});
            createTokenSetup.Returns(new Token());

            await registrationService.Register(new UserRegistration());

            publisher.Verify(p => p.Publish(EventType.NewUser, userId), Times.Once);
            publisher.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task SendConfirmationLetter()
        {
            var tokenId = Guid.Parse("644c6b5a-3629-497f-a7f9-b5a9c0cb9a90");
            createUserSetup.Returns(new User {Login = "login", Email = "e@mail.com"});
            createTokenSetup.Returns(new Token {TokenId = tokenId});

            await registrationService.Register(new UserRegistration());

            mailSender.Verify(s => s.Send("e@mail.com", "login", tokenId));
        }
    }
}