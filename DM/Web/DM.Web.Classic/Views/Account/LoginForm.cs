using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Account
{
    public class LoginForm
    {
        [Required(ErrorMessage = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        public bool DoNotRemember { get; set; }

        public string RedirectUrl { get; set; }
    }
}