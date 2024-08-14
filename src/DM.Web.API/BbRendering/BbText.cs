namespace DM.Web.API.BbRendering;

/// <summary>
/// BB text
/// </summary>
public abstract class BbText
{
    /// <summary>
    /// Text
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Parse mode
    /// </summary>
    public abstract BbParseMode ParseMode { get; }
}

/// <inheritdoc />
public class ChatBbText : BbText
{
    /// <inheritdoc />
    public override BbParseMode ParseMode => BbParseMode.Chat;
}

/// <inheritdoc />
public class PostBbText : BbText
{
    /// <inheritdoc />
    public override BbParseMode ParseMode => BbParseMode.Post;
}

/// <inheritdoc />
public class CommonBbText : BbText
{
    /// <inheritdoc />
    public override BbParseMode ParseMode => BbParseMode.Common;
}

/// <inheritdoc />
public class InfoBbText : BbText
{
    /// <inheritdoc />
    public override BbParseMode ParseMode => BbParseMode.Info;
}