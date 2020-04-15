using System.ComponentModel.DataAnnotations;

namespace DM.Web.Classic.Views.Profile.EditProfileCredentials
{
    public class EditPasswordForm
    {
        [Required(ErrorMessage = "Введите свой старый пароль")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Пароль должен быть хотя бы 7 символов длиной")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Введите новый пароль")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "Пароль должен быть хотя бы 7 символов длиной")]
        public string NewPasswordConfirmation { get; set; }
    }
}