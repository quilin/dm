namespace DM.Web.Classic.Views.Account
{
    public interface IUserActionsViewModelBuilder
    {
        UserActionsViewModel Build(string login);
    }
}