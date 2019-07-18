using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Common.Authorization;
using DM.Services.Core.Dto;
using DM.Services.Core.Extensions;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Moderation;
using DM.Services.Forum.BusinessProcesses.Topics.Reading;
using DM.Services.Forum.Dto.Output;
using DM.Web.Classic.Views.Fora.CreateTopic;
using DM.Web.Classic.Views.Fora.Topic;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Fora
{
    public class ForumViewModelBuilder : IForumViewModelBuilder
    {
        private readonly ITopicReadingService topicReadingService;
        private readonly IModeratorsReadingService moderatorsReadingService;
        private readonly ITopicViewModelBuilder topicViewModelBuilder;
        private readonly IIntentionManager intentionsManager;
        private readonly ICreateTopicFormBuilder createTopicFormBuilder;
        private readonly IUserViewModelBuilder userViewModelBuilder;
        private readonly IIdentity identity;

        public ForumViewModelBuilder(
            ITopicReadingService topicReadingService,
            IModeratorsReadingService moderatorsReadingService,
            ITopicViewModelBuilder topicViewModelBuilder,
            IIntentionManager intentionsManager,
            ICreateTopicFormBuilder createTopicFormBuilder,
            IUserViewModelBuilder userViewModelBuilder,
            IIdentityProvider identityProvider)
        {
            this.topicReadingService = topicReadingService;
            this.moderatorsReadingService = moderatorsReadingService;
            this.topicViewModelBuilder = topicViewModelBuilder;
            this.intentionsManager = intentionsManager;
            this.createTopicFormBuilder = createTopicFormBuilder;
            this.userViewModelBuilder = userViewModelBuilder;
            identity = identityProvider.Current;
        }

        public async Task<ForumViewModel> Build(Forum forum, int entityNumber)
        {
            var (canCreateTopic, (topics, paging), attachedTopics, moderators) = await AsyncExtensions.WhenAll(
                intentionsManager.IsAllowed(ForumIntention.CreateTopic, forum),
                topicReadingService.GetTopicsList(forum.Title, new PagingQuery {Number = entityNumber}),
                topicReadingService.GetAttachedTopics(forum.Title),
                moderatorsReadingService.GetModerators(forum.Title));

            return new ForumViewModel
            {
                ForumId = forum.Id,
                Title = forum.Title,
                Moderators = moderators.Select(userViewModelBuilder.Build).ToArray(),
                AttachedTopics = attachedTopics.Select(topicViewModelBuilder.Build).ToArray(),
                Topics = topics.Select(topicViewModelBuilder.Build).ToArray(),

                CurrentPage = paging.CurrentPage,
                TotalPagesCount = paging.TotalPagesCount,
                PageSize = paging.PageSize,
                EntityNumber = paging.EntityNumber,

                CanCreateTopic = canCreateTopic,
                CanMarkAsRead = identity.User.IsAuthenticated,
                CreateForm = canCreateTopic ? createTopicFormBuilder.Build(forum) : null
            };
        }

        public async Task<IEnumerable<TopicViewModel>> BuildList(Forum forum, int entityNumber)
        {
            var (topics, _) = await topicReadingService.GetTopicsList(forum.Title, new PagingQuery
            {
                Number = entityNumber,
                Size = identity.Settings.TopicsPerPage
            });

            return topics.Select(topicViewModelBuilder.Build);
        }
    }
}