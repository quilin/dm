using System;

namespace DM.Services.Community.Dto
{
    /// <summary>
    /// DTO model for password update
    /// </summary>
    public class UserPasswordChange
    {
        /// <summary>
        /// Login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Password reset token
        /// </summary>
        public Guid? Token { get; set; }

        /// <summary>
        /// Old password
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// New password
        /// </summary>
        public string NewPassword { get; set; }
    }
}