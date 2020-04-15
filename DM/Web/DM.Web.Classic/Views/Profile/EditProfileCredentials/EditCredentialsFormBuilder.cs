namespace DM.Web.Classic.Views.Profile.EditProfileCredentials
{
    public class EditCredentialsFormBuilder : IEditCredentialsFormBuilder
    {
        public EditEmailForm BuildEmail()
        {
            return new EditEmailForm();
        }

        public EditPasswordForm BuildPassword()
        {
            return new EditPasswordForm();
        }
    }
}