using System;

namespace DM.Services.Core.Implementation
{
    /// <inheritdoc />
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.UtcNow;
    }
}