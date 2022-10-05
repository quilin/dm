using System.Threading.Tasks;
using DM.Web.API.Dto.Messaging;

namespace DM.Web.API.Services.Community;

/// <summary>
/// Hub service for community chat
/// </summary>
internal interface IChatHubClient
{
    /// <summary>
    /// Send new message to all connected users
    /// </summary>
    /// <param name="chatMessage"></param>
    /// <returns></returns>
    Task SendMessage(ChatMessage chatMessage);
}