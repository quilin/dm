using System;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICommentariesViewModelBuilder
    {
        CommentariesViewModel Build(Guid entityId, int? entityNumber);
        CommentaryViewModel[] BuildList(Guid entityId, int? entityNumber);
    }
}