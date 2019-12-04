using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public class EditGameStatusFormBuilder : IEditGameStatusFormBuilder
    {
        public EditGameStatusForm Build(Game game)
        {
            return new EditGameStatusForm
            {
                GameId = game.Id,
                Status = game.Status
            };
        }
    }
}