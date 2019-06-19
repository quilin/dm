using System.Linq;
using DM.Services.Common.Authorization;
using DM.Services.Forum.Authorization;
using DM.Services.Forum.BusinessProcesses.Fora;

namespace DM.Web.Classic.Views.Topic
{
    public class TopicActionsViewModelBuilder : ITopicActionsViewModelBuilder
    {
        private readonly IIntentionManager intentionsManager;
        private readonly IForumReadingService forumReadingService;

        public TopicActionsViewModelBuilder(
            IIntentionManager intentionsManager,
            IForumReadingService forumReadingService
        )
        {
            this.intentionsManager = intentionsManager;
            this.forumReadingService = forumReadingService;
        }

        public TopicActionsViewModel Build(Services.Forum.Dto.Output.Topic topic)
        {
            return new TopicActionsViewModel
            {
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                CanAttach = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                CanDetach = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                CanClose = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                CanOpen = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                CanEdit = intentionsManager.IsAllowed(TopicIntention.Edit, topic).Result,
                CanRemove = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                CanMove = intentionsManager.IsAllowed(ForumIntention.AdministrateTopics, topic.Forum).Result,
                Forums = forumReadingService.GetForaList().Result.ToDictionary(f => f.Title, f => (object) f.Id)
            };
        }
    }
}