using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BBCodeParser;
using DM.Services.Core.Dto.Enums;

namespace DM.Web.Classic.Views.Profile.Settings
{
    public class UserSettingsForm
    {
        public Dictionary<string, string> TimezonesList { get; set; }
        public string Timezone { get; set; }

        [Required(ErrorMessage = "Введите число сообщений на странице")]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть хотя бы 1 сообщение на странице")]
        public int PostsPerPage { get; set; }

        [Required(ErrorMessage = "Введите число комментариев на странице")]
        [Range(1, int.MaxValue, ErrorMessage = "Должен быть хотя бы 1 комментарий на странице")]
        public int CommentsPerPage { get; set; }

        [Required(ErrorMessage = "Введите число тем на странице")]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть хотя бы 1 тема на странице")]
        public int TopicsPerPage { get; set; }

        [Required(ErrorMessage = "Введите число сообщений на странице")]
        [Range(1, int.MaxValue, ErrorMessage = "Должно быть хотя бы 1 сообщение на странице")]
        public int MessagesPerPage { get; set; }
        
        [Required(ErrorMessage = "Введите число прочих сущностей на странице")]
        [Range(1, int.MaxValue, ErrorMessage = "Должна быть хотя бы 1 сущность на странице")]
        public int EntitiesPerPage { get; set; }

        public bool CanEditNurseGreetingsMessage { get; set; }
        public string NurseGreetingsMessage { get; set; }

        public ColorSchema ColorSchema { get; set; }

        public bool RatingDisabled { get; set; }

        public IBbParser Parser { get; set; }
    }
}