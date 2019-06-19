namespace DM.Web.Classic.Views.Error
{
    public interface IErrorViewModelBuilder
    {
        ErrorViewModel Build(int statusCode, string path);
    }
}