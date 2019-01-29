using BBCodeParser;

namespace DM.Web.Core.Parsers
{
    public class BbParserProvider : IBbParserProvider
    {
        public BbParserProvider(
            IBbParserFactory bbParserFactory)
        {
            CurrentCommon = bbParserFactory.CreateCommon();
            CurrentInfo = bbParserFactory.CreateInfo();
            CurrentConversationMessage = bbParserFactory.CreateConversationMessage();
            CurrentPost = bbParserFactory.CreatePost();
            CurrentSafePost = bbParserFactory.CreateSafePost();
            CurrentSafeRating = bbParserFactory.CreateSafeRating();
            CurrentGeneralChat = bbParserFactory.CreateGeneralChat();
        }

        public IBbParser CurrentCommon { get; }

        public IBbParser CurrentInfo { get; }

        public IBbParser CurrentConversationMessage { get; }

        public IBbParser CurrentPost { get; }

        public IBbParser CurrentSafePost { get; }

        public IBbParser CurrentSafeRating { get; }

        public IBbParser CurrentGeneralChat { get; }
    }
}