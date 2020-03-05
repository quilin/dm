using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading
{
    /// <summary>
    /// Service for reading user conversations
    /// </summary>
    public interface IConversationReadingService
    {
        /// <summary>
        /// Get list of user conversations
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <returns></returns>
        Task<(IEnumerable<Conversation> conversations, PagingResult paging)> Get(PagingQuery query);

        /// <summary>
        /// Get single conversation
        /// </summary>
        /// <param name="id">Conversation identifier</param>
        /// <returns></returns>
        Task<Conversation> Get(Guid id);

        /// <summary>
        /// Find user visavi conversation
        /// </summary>
        /// <param name="login">User login</param>
        /// <returns></returns>
        Task<Conversation> Get(string login);
    }
}