using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;
using DM.Services.DataAccess.RelationalStorage;
using DbMessage = DM.Services.DataAccess.BusinessObjects.Messaging.Message;
using DbConversation = DM.Services.DataAccess.BusinessObjects.Messaging.Conversation;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <summary>
/// Storage for message creating
/// </summary>
internal interface IMessageCreatingRepository
{
    /// <summary>
    /// Save message
    /// </summary>
    /// <param name="message"></param>
    /// <param name="updateConversation"></param>
    /// <returns></returns>
    Task<Message> Create(DbMessage message, IUpdateBuilder<DbConversation> updateConversation);
}