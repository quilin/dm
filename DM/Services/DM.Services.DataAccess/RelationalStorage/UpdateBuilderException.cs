using System;

namespace DM.Services.DataAccess.RelationalStorage
{
    /// <inheritdoc />
    public class UpdateBuilderException : Exception
    {
        /// <inheritdoc />
        public UpdateBuilderException(string message) : base(message)
        {
        }
    }
}