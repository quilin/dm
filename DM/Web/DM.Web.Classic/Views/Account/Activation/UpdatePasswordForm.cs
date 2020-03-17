using System;
using System.ComponentModel.DataAnnotations;
using DM.Web.Classic.Views.Shared.User;

namespace DM.Web.Classic.Views.Account.Activation
{
    public class UpdatePasswordForm
    {
        [Required(ErrorMessage = "Введите пароль")]
        public string AlteredPassword { get; set; }

        public Guid Token { get; set; }

        public UserViewModel User { get; set; }
    }
}