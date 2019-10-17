using System.Threading.Tasks;
using DM.Services.Common.Dto;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface ICommentaryViewModelBuilder
    {
        Task<CommentaryViewModel> Build(Comment comment);
    }
}