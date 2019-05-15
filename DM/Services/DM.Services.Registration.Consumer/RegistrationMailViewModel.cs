using System;

namespace DM.Services.Registration.Consumer
{
    /// <summary>
    /// View model for user activation email
    /// </summary>
    public class RegistrationMailViewModel
    {
        /// <summary>
        /// New user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// New user login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Activation token
        /// </summary>
        public Guid Token { get; set; }
    }
}