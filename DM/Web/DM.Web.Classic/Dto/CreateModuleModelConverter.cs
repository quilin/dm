using System;
using DM.Web.Classic.Views.CreateModule;

namespace DM.Web.Classic.Dto
{
    public class CreateModuleModelConverter : ICreateModuleModelConverter
    {
        public CreateModuleModel Convert(CreateModuleForm form)
        {
            return new CreateModuleModel
            {
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
                DisableAlignment = form.DisableAlignment,
                ShowPrivateMessages = form.ShowPrivateMessages,
                TagIds = form.TagIds ?? new Guid[0],
                CreateAsRegistration = form.CreateAsRegistration
            };
        }
    }
}