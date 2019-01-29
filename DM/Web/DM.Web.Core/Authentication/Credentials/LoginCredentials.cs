namespace DM.Web.Core.Authentication.Credentials
{
    public class LoginCredentials : AuthCredentials
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}