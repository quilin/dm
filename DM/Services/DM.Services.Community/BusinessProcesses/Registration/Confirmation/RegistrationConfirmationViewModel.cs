namespace DM.Services.Community.BusinessProcesses.Registration.Confirmation
{
    /// <summary>
    /// View model for registration confirmation letter
    /// </summary>
    public class RegistrationConfirmationViewModel
    {
        /// <summary>
        /// Registered user login
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Link to activate the user
        /// </summary>
        public string ConfirmationLinkUrl { get; set; }
    }
}