using System;
using DM.Web.Classic.Views.EditModule;

namespace DM.Web.Classic.Dto
{
    public class UpdateModuleModelConverter : IUpdateModuleModelConverter
    {
        public UpdateModuleModel Convert(EditModuleForm form)
        {
            return new UpdateModuleModel
            {
                ModuleId = form.ModuleId,
                Title = form.Title,
                SystemName = form.SystemName,
                SettingName = form.SettingName,
                Info = form.Info,
                AssistantId = form.AssistantId,
                AttributeSchemeId = form.AttributeSchemeId == ModuleAttributes.NoScheme ? (Guid?)null : form.AttributeSchemeId,
                CommentariesAccessMode = form.CommentariesAccessMode,
                HideTemper = form.HideTemper,
                HideSkills = form.HideSkills,
                HideInventory = form.HideInventory,
                HideStory = form.HideStory,
                HideDiceResults = form.HideDiceResults,
                ShowPrivateMessages = form.ShowPrivateMessages,
                DisableAlignment = form.DisableAlignment,
                TagIds = form.TagIds ?? new Guid[0]
            };
        }
    }
}