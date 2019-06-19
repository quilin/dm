using System;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public class ForaListItemViewModelBuilder : IForaListItemViewModelBuilder
    {
        public ForaListItemViewModel Build(Forum forum, Guid? forumId)
        {
            return new ForaListItemViewModel
            {
                IsCurrent = forumId == forum.Id,
                Title = forum.Title,
                ForumId = forum.Id,
                UnreadTopicsCount = forum.UnreadTopicsCount,
            };
        }
    }
}