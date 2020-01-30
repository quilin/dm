namespace DM.Services.Community.BusinessProcesses.EmailChange.Confirmation
{
    /// <summary>
    /// View model for registration confirmation letter
    /// </summary>
    public class EmailChangeConfirmationViewModel
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