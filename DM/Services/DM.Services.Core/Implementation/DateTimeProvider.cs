using System;

namespace DM.Services.Core.Implementation
{
    /// <inheritdoc />
    public class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.UtcNow;

        /// <inheritdoc />
        public DateTime Ago(TimeSpan timeSpan) => Now - timeSpan;

        /// <inheritdoc />
        public DateTime Later(TimeSpan timeSpan) => Now + timeSpan;
    }
}