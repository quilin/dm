using AutoMapper;
using DM.Services.Core.Dto.Enums;
using DM.Services.Gaming.Dto.Input;
using DM.Web.Core.Helpers;

namespace DM.Web.API.Dto.Games
{
    /// <summary>
    /// Mapping profile for game models
    /// </summary>
    public class GameProfile : Profile
    {
        /// <inheritdoc />
        public GameProfile()
        {
            CreateMap<DM.Services.Gaming.Dto.Output.Game, Game>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.Id.EncodeToReadable(g.Title)))
                .ForMember(d => d.System, s => s.MapFrom(g => g.SystemName))
                .ForMember(d => d.Setting, s => s.MapFrom(g => g.SettingName))
                .ForMember(d => d.Released, s => s.MapFrom(g => g.ReleaseDate ?? g.CreateDate))
                .ForMember(d => d.Participation, s => s.MapFrom<GameParticipationResolver>());

            CreateMap<DM.Services.Gaming.Dto.Output.GameExtended, Game>()
                .ForMember(d => d.PrivacySettings, s => s.MapFrom(g => g))
                .ForMember(d => d.Notes, s => s.MapFrom(g => g.Notepad));

            CreateMap<DM.Services.Gaming.Dto.Output.GameExtended, GamePrivacySettings>()
                .ForMember(d => d.ViewTemper, s => s.MapFrom(g => !g.HideTemper))
                .ForMember(d => d.ViewStory, s => s.MapFrom(g => !g.HideStory))
                .ForMember(d => d.ViewSkills, s => s.MapFrom(g => !g.HideSkills))
                .ForMember(d => d.ViewInventory, s => s.MapFrom(g => !g.HideInventory))
                .ForMember(d => d.ViewPrivates, s => s.MapFrom(g => g.ShowPrivateMessages))
                .ForMember(d => d.ViewDice, s => s.MapFrom(g => !g.HideDiceResult))
                .ForMember(d => d.CommentariesAccess, s => s.MapFrom(g => g.CommentariesAccessMode));

            CreateMap<Game, CreateGame>()
                .ForMember(g => g.SystemName, s => s.MapFrom(g => g.System))
                .ForMember(g => g.SettingName, s => s.MapFrom(g => g.Setting))
                .ForMember(g => g.Info, s => s.MapFrom(g => g.Info))
                .ForMember(g => g.AssistantLogin, s => s.MapFrom(g => g.Assistant.Login))
                .ForMember(g => g.Draft, s => s.MapFrom(g => g.Status == GameStatus.Draft))
                .ForMember(g => g.HideTemper, s => s.MapFrom(g => !g.PrivacySettings.ViewTemper))
                .ForMember(g => g.HideStory, s => s.MapFrom(g => !g.PrivacySettings.ViewStory))
                .ForMember(g => g.HideSkills, s => s.MapFrom(g => !g.PrivacySettings.ViewSkills))
                .ForMember(g => g.HideInventory, s => s.MapFrom(g => !g.PrivacySettings.ViewInventory))
                .ForMember(g => g.HideDiceResult, s => s.MapFrom(g => !g.PrivacySettings.ViewDice))
                .ForMember(g => g.ShowPrivateMessages, s => s.MapFrom(g => g.PrivacySettings.ViewPrivates))
                .ForMember(g => g.CommentariesAccessMode, s => s.MapFrom(g => g.PrivacySettings.CommentariesAccess));
        }
    }
}