using System;
using DM.Services.Common.Dto;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace DM.Services.Common.Tests;

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
    public void ThrowValidationExceptionWhenTopicIdIsEmpty()
    {
        validator.Invoking(v => v.ValidateAndThrowAsync(new CreateComment {Text = "something"}).Wait())
            .Should().Throw<ValidationException>()
            .And.Errors.Should().ContainSingle(e => e.PropertyName == "EntityId");
    }

    [Fact]
    public void NotThrowWhenAllOk()
    {
        validator.Invoking(v => v.ValidateAndThrowAsync(new CreateComment
            {
                Text = "something",
                EntityId = Guid.NewGuid()
            }).Wait())
            .Should().NotThrow();
    }
}