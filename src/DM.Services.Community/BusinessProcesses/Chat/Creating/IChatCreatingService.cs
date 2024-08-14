using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Chat.Reading;

namespace DM.Services.Community.BusinessProcesses.Chat.Creating;

/// <summary>
/// Service for creating chat messages
/// </summary>
public interface IChatCreatingService
{
    /// <summary>
    /// Create new chat message
    /// </summary>
    /// <param name="createChatMessage">Creating DTO</param>
    /// <returns></returns>
    Task<ChatMessage> Create(CreateChatMessage createChatMessage);
}