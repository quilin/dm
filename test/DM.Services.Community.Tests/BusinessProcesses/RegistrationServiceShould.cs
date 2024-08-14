using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.Security;
using DM.Services.Community.BusinessProcesses.Account.Activation;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Services.Community.BusinessProcesses.Account.Registration.Confirmation;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Tests.Core;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses;

public class RegistrationServiceShould : UnitTestBase
{
    private readonly ISetup<ISecurityManager, (string Hash, string Salt)> passwordGenerationSetup;
    private readonly ISetup<IUserFactory, User> createUserSetup;
    private readonly ISetup<IActivationTokenFactory, Token> createTokenSetup;
    private readonly Mock<IUserFactory> userFactory;
    private readonly Mock<IActivationTokenFactory> tokenFactory;
    private readonly Mock<IRegistrationRepository> registrationRepository;
    private readonly Mock<IRegistrationMailSender> mailSender;
    private readonly Mock<IInvokedEventProducer> eventPublisher;
    private readonly RegistrationService service;
    private readonly Mock<ISecurityManager> securityManager;

    public RegistrationServiceShould()
    {
        var validator = Mock<IValidator<UserRegistration>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UserRegistration>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        securityManager = Mock<ISecurityManager>();
        passwordGenerationSetup = securityManager.Setup(m => m.GeneratePassword(It.IsAny<string>()));

        userFactory = Mock<IUserFactory>();
        createUserSetup = userFactory.Setup(f =>
            f.Create(It.IsAny<UserRegistration>(), It.IsAny<string>(), It.IsAny<string>()));

        tokenFactory = Mock<IActivationTokenFactory>();
        createTokenSetup = tokenFactory.Setup(f => f.Create(It.IsAny<Guid>()));

        registrationRepository = Mock<IRegistrationRepository>();
        registrationRepository
            .Setup(r => r.AddUser(It.IsAny<User>(), It.IsAny<Token>()))
            .Returns(Task.CompletedTask);

        mailSender = Mock<IRegistrationMailSender>();
        mailSender
            .Setup(s => s.Send(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        eventPublisher = Mock<IInvokedEventProducer>();
        eventPublisher
            .Setup(p => p.Send(It.IsAny<EventType>(), It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        service = new RegistrationService(validator.Object,
            securityManager.Object,
            userFactory.Object,
            tokenFactory.Object,
            registrationRepository.Object,
            mailSender.Object,
            eventPublisher.Object);
    }

    [Fact]
    public async Task CreateUserWithGeneratedSaltAndHash()
    {
        passwordGenerationSetup.Returns(("hash", "salt"));
        createTokenSetup.Returns(new Token());
        createUserSetup.Returns(new User());

        var userRegistration = new UserRegistration {Password = "my password"};
        await service.Register(userRegistration);

        securityManager.Verify(m => m.GeneratePassword("my password"));
        userFactory.Verify(f => f.Create(userRegistration, "salt", "hash"));
    }

    [Fact]
    public async Task CreateTokenWithGeneratedUserId()
    {
        passwordGenerationSetup.Returns(("hash", "salt"));
        createTokenSetup.Returns(new Token());
        var userId = Guid.NewGuid();
        createUserSetup.Returns(new User {UserId = userId});

        await service.Register(new UserRegistration());

        tokenFactory.Verify(f => f.Create(userId));
    }

    [Fact]
    public async Task SaveCreatedUserAndToken()
    {
        passwordGenerationSetup.Returns(("hash", "salt"));
        var token = new Token();
        createTokenSetup.Returns(token);
        var user = new User();
        createUserSetup.Returns(user);

        await service.Register(new UserRegistration());

        registrationRepository.Verify(r => r.AddUser(user, token), Times.Once);
        registrationRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task SendConfirmationLetter()
    {
        passwordGenerationSetup.Returns(("hash", "salt"));
        var tokenId = Guid.NewGuid();
        createTokenSetup.Returns(new Token {TokenId = tokenId});
        createUserSetup.Returns(new User {Email = "email", Login = "login"});

        await service.Register(new UserRegistration());

        mailSender.Verify(s => s.Send("email", "login", tokenId), Times.Once);
        mailSender.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task PublishEvent()
    {
        passwordGenerationSetup.Returns(("hash", "salt"));
        createTokenSetup.Returns(new Token());
        var userId = Guid.NewGuid();
        createUserSetup.Returns(new User {UserId = userId});

        await service.Register(new UserRegistration {Email = "email", Login = "login"});

        eventPublisher.Verify(p => p.Send(EventType.NewUser, userId));
    }
}