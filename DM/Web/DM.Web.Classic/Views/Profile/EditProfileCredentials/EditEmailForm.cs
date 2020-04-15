using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Profile.EditProfileCredentials
{
    public class EditEmailForm
    {
        [Required(ErrorMessage = "Введите пароль от вашей учетной записи")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите новый e-mail")]
        public string NewEmail { get; set; }
    }
}