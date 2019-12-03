using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public interface IEditGameStatusFormBuilder
    {
        EditGameStatusForm Build(Game game);
    }
}