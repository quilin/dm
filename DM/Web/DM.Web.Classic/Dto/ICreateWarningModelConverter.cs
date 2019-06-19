using DM.Web.Classic.Views.Shared.Warnings.CreateWarning;

namespace DM.Web.Classic.Dto
{
    public interface ICreateWarningModelConverter
    {
        CreateWarningModel Convert(CreateWarningForm form);
    }
}