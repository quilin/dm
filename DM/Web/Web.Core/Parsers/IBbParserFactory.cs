using BBCodeParser;

namespace Web.Core.Parsers
{
    public interface IBbParserFactory
    {
        IBbParser CreateCommon();
        IBbParser CreateConversationMessage();
        IBbParser CreateInfo();
        IBbParser CreatePost();
        IBbParser CreateSafePost();
        IBbParser CreateSafeRating();
        IBbParser CreateGeneralChat();
    }
}