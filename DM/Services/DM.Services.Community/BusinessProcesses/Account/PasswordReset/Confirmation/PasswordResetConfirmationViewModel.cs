namespace DM.Services.Community.BusinessProcesses.Account.PasswordReset.Confirmation
{
    /// <summary>
    /// View model for password reset confirmation letter
    /// </summary>
    public class PasswordResetConfirmationViewModel
    {
        /// <summary>
        /// User login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Confirmation link URL
        /// </summary>
        public string ConfirmationLinkUri { get; set; }
    }
}