using System;

namespace DM.Services.Core.Implementation;

/// <inheritdoc />
internal class GuidFactory : IGuidFactory
{
    /// <inheritdoc />
    public Guid Create() => Guid.NewGuid();
}