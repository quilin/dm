using System;
using System.ComponentModel.DataAnnotations;
using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.Classic.Views.Account.Activation
{
    public class UpdatePasswordForm
    {
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль еще раз")]
        public string PasswordConfirmation { get; set; }

        public Guid TokenId { get; set; }

        public TokenType TokenType { get; set; }
        public string Login { get; set; }
    }
}