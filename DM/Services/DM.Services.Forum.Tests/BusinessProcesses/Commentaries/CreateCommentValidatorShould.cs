using DM.Services.Forum.Dto.Input;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace DM.Services.Forum.Tests.BusinessProcesses.Commentaries
{
    public class CreateCommentValidatorShould
    {
        private readonly CreateCommentValidator validator;

        public CreateCommentValidatorShould()
        {
            validator = new CreateCommentValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ThrowValidationExceptionWhenTextIsEmpty(string text)
        {
            validator.Invoking(v => v.ValidateAndThrowAsync(new CreateComment {Text = text}).Wait())
                .Should().Throw<ValidationException>()
                .And.Errors.Should().ContainSingle(e => e.PropertyName == "Text");
        }

        [Fact]
        public void NotThrowWhenTextIsOk()
        {
            validator.Invoking(v => v.ValidateAndThrowAsync(new CreateComment {Text = "something"}).Wait())
                .Should().NotThrow();
        }
    }
}