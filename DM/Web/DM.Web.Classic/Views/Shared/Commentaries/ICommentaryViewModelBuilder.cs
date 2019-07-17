using System.Threading.Tasks;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICommentaryViewModelBuilder
    {
        Task<CommentaryViewModel> Build(Comment comment);
    }
}