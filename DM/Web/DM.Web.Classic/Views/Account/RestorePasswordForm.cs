using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Account
{
    public class RestorePasswordForm
    {
        [Required(ErrorMessage = "Введите свой e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите свой логин")]
        public string Login { get; set; }
    }
}