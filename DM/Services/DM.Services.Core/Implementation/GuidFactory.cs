using System;

namespace DM.Services.Core.Implementation
{
    /// <inheritdoc />
    public class GuidFactory : IGuidFactory
    {
        /// <inheritdoc />
        public Guid Create() => Guid.NewGuid();
    }
}