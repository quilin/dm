using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.GameCommentaries.CreateCommentary
{
    public interface IGameCommentaryCreateFormBuilder
    {
        GameCommentaryCreateForm Build(Game game);
    }
}