using DM.Services.DataAccess.BusinessObjects.Messaging;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating
{
    /// <summary>
    /// Factory for message DAL model
    /// </summary>
    public interface IMessageFactory
    {
        /// <summary>
        /// Create new message DAL model
        /// </summary>
        /// <param name="createMessage"></param>
        /// <returns></returns>
        Message Create(CreateMessage createMessage);
    }
}