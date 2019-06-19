using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICommentaryViewModelBuilder
    {
        CommentaryViewModel Build(Comment comment);
    }
}