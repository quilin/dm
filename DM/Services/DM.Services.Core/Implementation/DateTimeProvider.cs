using System;

namespace DM.Services.Core.Implementation
{
    /// <inheritdoc />
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTimeOffset Now => DateTimeOffset.UtcNow;
    }
}