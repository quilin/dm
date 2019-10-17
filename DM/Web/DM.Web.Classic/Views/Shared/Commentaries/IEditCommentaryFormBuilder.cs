using DM.Services.Common.Dto;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public interface IEditCommentaryFormBuilder
    {
        EditCommentaryForm Build(Comment commentary);
    }
}