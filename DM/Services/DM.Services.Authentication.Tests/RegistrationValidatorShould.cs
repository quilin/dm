using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Dto.Validations;
using FluentAssertions;
using Xunit;

namespace DM.Services.Authentication.Tests
{
    public class RegistrationValidatorShould
    {
        private readonly UserRegistrationValidator validator;

        public RegistrationValidatorShould()
        {
            validator = new UserRegistrationValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData((string) null)]
        [InlineData("  ")]
        [InlineData("the very long string that could not be user's login in any way")]
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
            (await validator.ValidateAsync(userRegistration)).IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData((string) null)]
        [InlineData("  ")]
        [InlineData("kajlsdhfaksjdhlfaksljdhfklasdhfaksdhadfasdfaslkdhfaskdjhfasldfaslkdjhaskdhfaskldfhaskjldfhaskdhfsakdhfaskldhf@gmail.com")]
        [InlineData("someInvalidEmail")]
        public async Task ValidateUserEmail(string email)
        {
            var userRegistration = new UserRegistration
            {
                Login = "User",
                Password = "qwerty",
                Email = email
            };
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
}