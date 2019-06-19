using System;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Shared.ForaList
{
    public interface IForaListItemViewModelBuilder
    {
        ForaListItemViewModel Build(Forum forum, Guid? forumId);
    }
}