using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto;

namespace DM.Services.Community.BusinessProcesses.Chat.Reading
{
    /// <summary>
    /// Service for reading chat messages
    /// </summary>
    public interface IChatReadingService
    {
        /// <summary>
        /// Get list of chat messages
        /// </summary>
        /// <returns></returns>
        Task<(IEnumerable<ChatMessage> messages, PagingResult paging)> GetMessages(PagingQuery pagingQuery);

        /// <summary>
        /// Get single chat message
        /// </summary>
        /// <param name="id">Message identifier</param>
        /// <returns></returns>
        Task<ChatMessage> GetMessage(Guid id);
    }
}