using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Account
{
    public class RegistrationForm
    {
        [Required(ErrorMessage = "Введите имя пользователя")]
        [MaxLength(30, ErrorMessage = "Имя не должно быть длиннее 30 символов")]
        [MinLength(3, ErrorMessage = "Имя должно состоять хотя бы из 3 символов")]
        [RegularExpression(@"^([a-zA-Zа-яА-Я0-9-_]+\s?)+$", ErrorMessage = "Имя содержит запрещенные символы")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите e-mail")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Выберите пароль")]
        [MinLength(6, ErrorMessage = "В пароле должно быть хотя бы 6 символов")]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }
    }
}