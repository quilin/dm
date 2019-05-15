using System;

namespace DM.Services.Core.Implementation
{
    /// <summary>
    /// Provides date in the same format across the application
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Current moment in UTC
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Moment some time ago in UTC
        /// </summary>
        /// <param name="timeSpan">Some time ago</param>
        /// <returns></returns>
        DateTime Ago(TimeSpan timeSpan);

        /// <summary>
        /// Moment some time later in UTC
        /// </summary>
        /// <param name="timeSpan">Some time later</param>
        /// <returns></returns>
        DateTime Later(TimeSpan timeSpan);
    }
}