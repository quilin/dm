using BBCodeParser;

namespace Web.Core.Parsers
{
    public interface IBbParserProvider
    {
        IBbParser CurrentCommon { get; }
        IBbParser CurrentInfo { get; }
        IBbParser CurrentConversationMessage { get; }
        IBbParser CurrentPost { get; }
        IBbParser CurrentSafePost { get; }
        IBbParser CurrentSafeRating { get; }
        IBbParser CurrentGeneralChat { get; }
    }
}