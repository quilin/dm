namespace DM.Services.Core.Implementation
{
    /// <summary>
    /// Factory for a password salt
    /// </summary>
    public interface ISaltFactory
    {
        /// <summary>
        /// Generates password salt of given length
        /// </summary>
        /// <param name="saltLength">Salt length</param>
        /// <returns>Random string for a password salt</returns>
        string Create(int saltLength);
    }
}