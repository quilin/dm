using System.Collections.Generic;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.EditGameStatus
{
    public class EditGameStatusViewModel
    {
        public EditGameStatusForm Form { get; set; }

        public Dictionary<GameStatus, bool> StatusCredentials { get; set; }

        public static readonly Dictionary<GameStatus, string> StatusDescriptions = new Dictionary<GameStatus, string>
        {
            {GameStatus.Draft, "вы еще не готовы показать свою игру миру (игроки не могут оставлять в ней сообщения, игра не выводится в списке \"Набор игроков\")"},
            {GameStatus.Requirement, "игра полностью готова &ndash; осталось набрать в нее людей"},
            {GameStatus.Active, "вся партия в сборе, вперед &ndash; на поиски приключений!"},
            {GameStatus.Frozen, "что-то пошло не так и хотя в настоящий момент игра \"заглохла\" вы полны желания продолжить ее чуть погодя"},
            {GameStatus.Finished, "игра пришла к своему логичному (или не очень) завершению. Вот это была история!"},
            {GameStatus.Closed, "к сожалению, игру не удалось довести до конца. Увы, такое случается с лучшими из нас :("},
        };
    }
}