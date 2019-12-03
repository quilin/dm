using DM.Services.Common.Dto;

namespace DM.Web.Classic.Views.GameCommentaries.EditCommentary
{
    public interface IGameCommentaryEditFormBuilder
    {
        GameCommentaryEditForm Build(Comment comment);
    }
}