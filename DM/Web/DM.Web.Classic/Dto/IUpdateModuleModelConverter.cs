using DM.Web.Classic.Views.EditModule;

namespace DM.Web.Classic.Dto
{
    public interface IUpdateModuleModelConverter
    {
        UpdateModuleModel Convert(EditModuleForm form);
    }
}