using System;
using DM.Services.Authentication.Dto;
using DM.Services.Core.Dto.Enums;

namespace DM.Services.Forum.Tests.Dsl
{
    public class AuthenticatedUserBuilder
    {
        private readonly AuthenticatedUser user;

        public AuthenticatedUserBuilder(Guid userId)
        {
            user = new AuthenticatedUser {UserId = userId};
        }

        public AuthenticatedUserBuilder WithRole(UserRole role)
        {
            user.Role = role;
            return this;
        }

        public AuthenticatedUser Please() => user;
    }
}