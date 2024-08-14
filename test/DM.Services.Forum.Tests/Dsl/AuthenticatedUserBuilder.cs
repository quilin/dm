using System;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Tests.Dsl;

public class AuthenticatedUserBuilder(Guid userId)
{
    private readonly AuthenticatedUser user = new() {UserId = userId};

    public AuthenticatedUserBuilder WithRole(UserRole role)
    {
        user.Role = role;
        return this;
    }

    public AuthenticatedUser Please() => user;
}