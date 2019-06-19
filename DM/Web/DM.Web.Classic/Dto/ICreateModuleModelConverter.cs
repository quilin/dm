using DM.Web.Classic.Views.CreateModule;

namespace DM.Web.Classic.Dto
{
    public interface ICreateModuleModelConverter
    {
        CreateModuleModel Convert(CreateModuleForm form);
    }
}