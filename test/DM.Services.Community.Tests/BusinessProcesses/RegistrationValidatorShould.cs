using System.Threading;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Account.Registration;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.Community.Tests.BusinessProcesses;

public class RegistrationValidatorShould : UnitTestBase
{
    private readonly UserRegistrationValidator validator;
    private readonly Mock<IRegistrationRepository> registrationRepository;

    public RegistrationValidatorShould()
    {
        registrationRepository = Mock<IRegistrationRepository>(MockBehavior.Loose);
        registrationRepository
            .Setup(r => r.EmailFree("EmailTaken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        registrationRepository
            .Setup(r => r.EmailFree(It.Is<string>(e => e != "EmailTaken"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        registrationRepository
            .Setup(r => r.LoginFree("LoginTaken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        registrationRepository
            .Setup(r => r.LoginFree(It.Is<string>(e => e != "LoginTaken"), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        validator = new UserRegistrationValidator(registrationRepository.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData((string) null)]
    [InlineData("  ")]
    [InlineData("the very long string that could not be user's login in any way")]
    [InlineData("LoginTaken")]
    public async Task ValidateUserLogin(string login)
    {
        var userRegistration = new UserRegistration
        {
            Login = login,
            Password = "qwerty",
            Email = "user@email.com"
        };
        (await validator.ValidateAsync(userRegistration)).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData((string) null)]
    [InlineData("  ")]
    [InlineData("short")]
    public async Task ValidateUserPassword(string password)
    {
        var userRegistration = new UserRegistration
        {
            Login = "User",
            Password = password,
            Email = "user@email.com"
        };
        registrationRepository
            .Setup(r => r.EmailFree(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        registrationRepository
            .Setup(r => r.LoginFree(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        (await validator.ValidateAsync(userRegistration)).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData((string) null)]
    [InlineData("  ")]
    [InlineData("kajlsdhfaksjdhlfaksljdhfklasdhfaksdhadfasdfaslkdhfaskdjhfasldfaslkdjhaskdhfaskldfhaskjldfhaskdhfsakdhfaskldhf@gmail.com")]
    [InlineData("someInvalidEmail")]
    [InlineData("EmailTaken")]
    public async Task ValidateUserEmail(string email)
    {
        var userRegistration = new UserRegistration
        {
            Login = "User",
            Password = "qwerty",
            Email = email
        };
        registrationRepository
            .Setup(r => r.LoginFree(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        (await validator.ValidateAsync(userRegistration)).IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task ValidateWholeModel()
    {
        (await validator.ValidateAsync(new UserRegistration
        {
            Email = "my@email.com",
            Password = "amazing_password",
            Login = "NewUser"
        })).IsValid.Should().BeTrue();
    }
}