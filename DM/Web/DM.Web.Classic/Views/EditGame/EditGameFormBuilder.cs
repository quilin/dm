using System.Linq;
using DM.Services.Core.Parsing;
using DM.Services.Gaming.Dto.Output;

namespace DM.Web.Classic.Views.EditGame
{
    public class EditGameFormBuilder : IEditGameFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public EditGameFormBuilder(
            IBbParserProvider bbParserProvider
        )
        {
            this.bbParserProvider = bbParserProvider;
        }

        public EditGameForm Build(GameExtended game)
        {
            var parser = bbParserProvider.CurrentInfo;
            return new EditGameForm
            {
                GameId = game.Id,
                Title = game.Title,
                SystemName = game.SystemName,
                SettingName = game.SettingName,
                AssistantId = game.Assistant?.UserId,
                Info = parser.Parse(game.Info).ToBb(),
                CommentariesAccessMode = game.CommentariesAccessMode,
                HideTemper = game.HideTemper,
                HideInventory = game.HideInventory,
                HideSkills = game.HideSkills,
                HideStory = game.HideStory,
                HideDiceResults = game.HideDiceResult,
                ShowPrivateMessages = game.ShowPrivateMessages,
                DisableAlignment = game.DisableAlignment,
                TagIds = game.Tags.Select(t => t.Id).ToArray(),
                Parser = parser
            };
        }
    }
}