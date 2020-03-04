using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Messaging;

namespace DM.Web.API.Services.Community
{
    /// <summary>
    /// API service for messaging
    /// </summary>
    public interface IMessagingApiService
    {
        /// <summary>
        /// Get list of user conversations
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <returns></returns>
        Task<ListEnvelope<Conversation>> GetConversations(PagingQuery query);

        /// <summary>
        /// Get single conversation
        /// </summary>
        /// <param name="id">Conversation identifier</param>
        /// <returns></returns>
        Task<Envelope<Conversation>> GetConversation(Guid id);

        /// <summary>
        /// Get list of conversation messages
        /// </summary>
        /// <param name="conversationId">Conversation identifier</param>
        /// <param name="query">Paging query</param>
        /// <returns></returns>
        Task<ListEnvelope<Message>> GetMessages(Guid conversationId, PagingQuery query);

        /// <summary>
        /// Get single message
        /// </summary>
        /// <param name="messageId">Message identifier</param>
        /// <returns></returns>
        Task<Envelope<Message>> GetMessage(Guid messageId);
    }
}