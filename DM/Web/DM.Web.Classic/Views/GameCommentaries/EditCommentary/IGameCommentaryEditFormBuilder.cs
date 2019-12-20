using System;
using System.Threading.Tasks;

namespace DM.Web.Classic.Views.GameCommentaries.EditCommentary
{
    public interface IGameCommentaryEditFormBuilder
    {
        Task<GameCommentaryEditForm> Build(Guid commentaryId);
    }
}