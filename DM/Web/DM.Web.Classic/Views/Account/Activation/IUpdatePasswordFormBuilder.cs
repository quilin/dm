using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.Classic.Views.Account.Activation
{
    public interface IUpdatePasswordFormBuilder
    {
        UpdatePasswordForm Build(Token token);
    }
}