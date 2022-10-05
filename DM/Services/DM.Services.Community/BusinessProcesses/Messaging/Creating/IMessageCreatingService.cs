using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Messaging.Reading;

namespace DM.Services.Community.BusinessProcesses.Messaging.Creating;

/// <summary>
/// Service for private message creating
/// </summary>
public interface IMessageCreatingService
{
    /// <summary>
    /// Create new message
    /// </summary>
    /// <param name="createMessage">DTO model</param>
    /// <returns></returns>
    Task<Message> Create(CreateMessage createMessage);
}