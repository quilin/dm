namespace DM.Services.DataAccess.BusinessObjects.Users
{
    /// <summary>
    /// Authorised action type
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// User registration password setting
        /// </summary>
        Registration = 0,

        /// <summary>
        /// Password restoration
        /// </summary>
        PasswordRestoration = 1
    }
}