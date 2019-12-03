using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGame
{
    public interface IEditGameFormBuilder
    {
        EditGameForm Build(GameExtended game);
    }
}