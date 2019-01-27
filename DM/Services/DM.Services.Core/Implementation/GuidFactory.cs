using System;

namespace DM.Services.Core.Implementation
{
    public class GuidFactory : IGuidFactory
    {
        public Guid Create() => Guid.NewGuid();
    }
}