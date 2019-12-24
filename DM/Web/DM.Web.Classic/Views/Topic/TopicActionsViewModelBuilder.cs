using System.Linq;
using System.Threading.Tasks;
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

        public async Task<TopicActionsViewModel> Build(Services.Forum.Dto.Output.Topic topic)
        {
            var administrationAllowed = await intentionsManager.IsAllowed(
                ForumIntention.AdministrateTopics, topic.Forum);
            return new TopicActionsViewModel
            {
                TopicId = topic.Id,
                TopicTitle = topic.Title,
                CanAttach = administrationAllowed,
                CanDetach = administrationAllowed,
                CanClose = administrationAllowed,
                CanOpen = administrationAllowed,
                CanEdit = await intentionsManager.IsAllowed(TopicIntention.Edit, topic),
                CanRemove = administrationAllowed,
                CanMove = administrationAllowed,
                Forums = (await forumReadingService.GetForaList()).ToDictionary(f => f.Title, f => (object) f.Title)
            };
        }
    }
}