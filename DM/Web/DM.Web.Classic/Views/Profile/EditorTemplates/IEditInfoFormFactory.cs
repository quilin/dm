namespace DM.Web.Classic.Views.Profile.EditorTemplates
{
    public interface IEditInfoFormFactory
    {
        EditInfoForm Create(string login, string info);
    }
}