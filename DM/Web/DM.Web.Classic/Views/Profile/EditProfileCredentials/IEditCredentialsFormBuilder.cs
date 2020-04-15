﻿namespace DM.Web.Classic.Views.Profile.EditProfileCredentials
{
    public interface IEditCredentialsFormBuilder
    {
        EditEmailForm BuildEmail();
        EditPasswordForm BuildPassword();
    }
}