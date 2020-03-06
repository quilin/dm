using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.BusinessProcesses.UnreadCounters;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;
using DM.Services.DataAccess.BusinessObjects.Common;

namespace DM.Services.Community.BusinessProcesses.Messaging.Reading
{
    /// <inheritdoc />
    public class ConversationReadingService : IConversationReadingService
    {
        private readonly IConversationFactory factory;
        private readonly IConversationReadingRepository repository;
        private readonly IUnreadCountersRepository unreadCountersRepository;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public ConversationReadingService(
            IConversationFactory factory,
            IConversationReadingRepository repository,
            IUnreadCountersRepository unreadCountersRepository,
            IIdentityProvider identityProvider)
        {
            this.factory = factory;
            this.repository = repository;
            this.unreadCountersRepository = unreadCountersRepository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<(IEnumerable<Conversation> conversations, PagingResult paging)> Get(PagingQuery query)
        {
            var totalCount = await repository.Count(identity.User.UserId);
            var pagingData = new PagingData(query, identity.Settings.Paging.MessagesPerPage, totalCount);
            var conversations = await repository.Get(identity.User.UserId, pagingData);

            return (conversations, pagingData.Result);
        }

        /// <inheritdoc />
        public async Task<Conversation> Get(Guid conversationId)
        {
            var conversation = await repository.Get(conversationId, identity.User.UserId);
            if (conversation == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Conversation not found");
            }

            return conversation;
        }

        /// <inheritdoc />
        public async Task<Conversation> GetOrCreate(string login)
        {
            var visaviId = await repository.FindUser(login);
            if (!visaviId.HasValue)
            {
                throw new HttpException(HttpStatusCode.Gone, "User not found");
            }

            var currentUserId = identity.User.UserId;
            var existingConversation = await repository.FindVisaviConversation(currentUserId, visaviId.Value);
            if (existingConversation != null)
            {
                return existingConversation;
            }

            var (conversation, conversationLinks) = factory.CreateVisavi(currentUserId, visaviId.Value);
            var result = await repository.Create(conversation, conversationLinks);

            foreach (var participantId in new[] {currentUserId, visaviId.Value}.Distinct())
            {
                await unreadCountersRepository.Create(result.Id, participantId, UnreadEntryType.Message);
            }

            return result;
        }
    }
}