using System.Collections.Generic;

namespace DM.Web.Classic.Views.Error
{
    public class ErrorViewModelBuilder : IErrorViewModelBuilder
    {
        private static readonly Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
        {
            [400] = "Неправильные параметры",
            [403] = "Доступ ограничен",
            [404] = "Страницы не существует",
            [410] = "Содержимое отсутствует",
            [500] = "Ошибка на сервере"
        };

        private static readonly Dictionary<int, string> ErrorTexts = new Dictionary<int, string>
        {
            [400] = "Вы отправили форму с некорректными данными или открыли \"битую\" ссылку.<br/>Великий Надмозг посчитал, что это может быть опасно для сайта и не дал вам закончить начатое.",
            [403] = "Вы попробовали совершить действие, на которое у вас нет разрешения.<br/>Скорее всего, вы просто забыли войти на сайт. Ну или вы злоумышленник, и за вами уже выхали, так и знайте!",
            [404] = "Возможно, вы допустили опечатку в адресе страницы или смеха ради написали там белиберду.",
            [410] = "Возможно, вы допустили опечатку в адресе страницы или раньше тут что-то было, но автор решил удалить содержимое.",
            [500] = "Это наш косяк. Но вы не переживайте, мы уже знаем об этой ошибке и предпринимаем все возможные меры. Кстати, иногда помогает "
        };

        public ErrorViewModel Build(int statusCode, string path)
        {
            return new ErrorViewModel
            {
                StatusCode = statusCode,
                Path = path,
                Message = ErrorMessages.TryGetValue(statusCode, out var error) ? error : "Случилась ошибка",
                Text = ErrorTexts.TryGetValue(statusCode, out var errorText) ? errorText : "Неведомый косяк."
            };
        }
    }
}