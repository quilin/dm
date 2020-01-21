using System;
using AutoMapper;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.Gaming.Dto.Input;
using DM.Services.Gaming.Dto.Output;
using DtoGame = DM.Services.Gaming.Dto.Output.Game;

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
            CreateMap<GamesQuery, DM.Services.Gaming.Dto.Input.GamesQuery>();

            CreateMap<DtoGame, Game>()
                .Include<GameExtended, Game>()
                .ForMember(d => d.Id, s => s.MapFrom(g => g.Id.EncodeToReadable(g.Title)))
                .ForMember(d => d.System, s => s.MapFrom(g => g.SystemName))
                .ForMember(d => d.Setting, s => s.MapFrom(g => g.SettingName))
                .ForMember(d => d.SchemaId, s => s.MapFrom<GuidResolver>())
                .ForMember(d => d.Released, s => s.MapFrom(g => g.ReleaseDate ?? g.CreateDate))
                .ForMember(d => d.Participation, s => s.MapFrom<GameParticipationResolver>());

            CreateMap<GameExtended, Game>()
                .ForMember(d => d.PrivacySettings, s => s.MapFrom(g => g))
                .ForMember(d => d.Notes, s => s.MapFrom(g => g.Notepad));

            CreateMap<GameExtended, GamePrivacySettings>()
                .ForMember(d => d.ViewTemper, s => s.MapFrom(g => !g.HideTemper))
                .ForMember(d => d.ViewStory, s => s.MapFrom(g => !g.HideStory))
                .ForMember(d => d.ViewSkills, s => s.MapFrom(g => !g.HideSkills))
                .ForMember(d => d.ViewInventory, s => s.MapFrom(g => !g.HideInventory))
                .ForMember(d => d.ViewPrivates, s => s.MapFrom(g => g.ShowPrivateMessages))
                .ForMember(d => d.ViewDice, s => s.MapFrom(g => !g.HideDiceResult))
                .ForMember(d => d.CommentariesAccess, s => s.MapFrom(g => g.CommentariesAccessMode));

            CreateMap<GameTag, Tag>()
                .ForMember(d => d.Id, s => s.MapFrom(t => t.Id.EncodeToReadable(t.Title)))
                .ForMember(d => d.Category, s => s.MapFrom(t => t.GroupTitle));

            CreateMap<Game, CreateGame>()
                .ForMember(g => g.SystemName, s => s.MapFrom(g => g.System))
                .ForMember(g => g.SettingName, s => s.MapFrom(g => g.Setting))
                .ForMember(g => g.AttributeSchemaId, s => s.MapFrom<GuidResolver>())
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

            CreateMap<Game, UpdateGame>()
                .ForMember(g => g.SystemName, s => s.MapFrom(g => g.System))
                .ForMember(g => g.SettingName, s => s.MapFrom(g => g.Setting))
                .ForMember(g => g.Info, s => s.MapFrom(g => g.Info))
                .ForMember(g => g.AssistantLogin, s => s.MapFrom(g => g.Assistant.Login))
                .ForMember(g => g.HideTemper, s => s.MapFrom(g => !g.PrivacySettings.ViewTemper))
                .ForMember(g => g.HideStory, s => s.MapFrom(g => !g.PrivacySettings.ViewStory))
                .ForMember(g => g.HideSkills, s => s.MapFrom(g => !g.PrivacySettings.ViewSkills))
                .ForMember(g => g.HideInventory, s => s.MapFrom(g => !g.PrivacySettings.ViewInventory))
                .ForMember(g => g.HideDiceResult, s => s.MapFrom(g => !g.PrivacySettings.ViewDice))
                .ForMember(g => g.ShowPrivateMessages, s => s.MapFrom(g => g.PrivacySettings.ViewPrivates))
                .ForMember(g => g.CommentariesAccessMode, s => s.MapFrom(g => g.PrivacySettings.CommentariesAccess));
        }
    }

    /// <inheritdoc />
    public class GuidResolver :
        IValueResolver<DtoGame, Game, string>,
        IValueResolver<Game, CreateGame, Guid?>
    {
        /// <inheritdoc />
        public string Resolve(DtoGame source, Game destination, string destMember, ResolutionContext context) =>
            source.AttributeSchemaId?.EncodeToReadable(string.Empty);

        /// <inheritdoc />
        public Guid? Resolve(Game source, CreateGame destination, Guid? destMember, ResolutionContext context) =>
            string.IsNullOrEmpty(source.SchemaId) ||
            !source.SchemaId.TryDecodeFromReadableGuid(out var id)
                ? (Guid?) null
                : id;
    }
}