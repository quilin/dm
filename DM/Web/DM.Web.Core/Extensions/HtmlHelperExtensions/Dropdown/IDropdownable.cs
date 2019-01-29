namespace DM.Web.Core.Extensions.HtmlHelperExtensions.Dropdown
{
    public interface IDropdownable
    {
        string GetDescription();
        string GetAdditionalData();
        string GetValue();
    }
}