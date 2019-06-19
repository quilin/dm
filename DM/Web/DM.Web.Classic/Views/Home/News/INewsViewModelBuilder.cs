namespace DM.Web.Classic.Views.Home.News
{
    public interface INewsViewModelBuilder
    {
        NewsViewModel Build(Services.Forum.Dto.Output.Topic topic);
    }
}