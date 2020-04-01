using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Chat.CreateMessage
{
    public class CreateChatMessageFormBuilder : ICreateChatMessageFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public CreateChatMessageFormBuilder(
            IBbParserProvider bbParserProvider)
        {
            this.bbParserProvider = bbParserProvider;
        }

        public CreateChatMessageForm Build() => new CreateChatMessageForm
        {
            Parser = bbParserProvider.CurrentCommon
        };
    }
}