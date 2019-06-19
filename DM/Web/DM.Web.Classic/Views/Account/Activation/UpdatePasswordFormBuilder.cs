using DM.Services.DataAccess.BusinessObjects.Users;

namespace DM.Web.Classic.Views.Account.Activation
{
    public class UpdatePasswordFormBuilder : IUpdatePasswordFormBuilder
    {
        public UpdatePasswordForm Build(Token token)
        {
            return new UpdatePasswordForm
            {
                TokenId = token.TokenId,
                TokenType = token.Type,
                Login = token.User.Login
            };
        }
    }
}