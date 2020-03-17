using DM.Services.Core.Dto;

namespace DM.Web.Classic.Views.Profile.Actions
{
    public interface IProfileActionsViewModelBuilder
    {
        ProfileActionsViewModel Build(GeneralUser user);
    }
}