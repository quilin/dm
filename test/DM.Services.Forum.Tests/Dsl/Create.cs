using System;

namespace DM.Services.Forum.Tests.Dsl;

public static class Create
{
    public static AuthenticatedUserBuilder User(Guid userId) => new(userId);
    public static AuthenticatedUserBuilder User() => new(Guid.NewGuid());
}