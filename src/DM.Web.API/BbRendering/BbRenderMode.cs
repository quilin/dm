namespace DM.Web.API.BbRendering;

/// <summary>
/// BB code rendering mode
/// </summary>
public enum BbRenderMode
{
    /// <summary>
    /// Render as HTML
    /// </summary>
    Html = 0,

    /// <summary>
    /// Render as BB-code (for editing)
    /// </summary>
    Bb = 1,

    /// <summary>
    /// Render as HTML without formatting
    /// </summary>
    Text = 2,

    /// <summary>
    /// Render as HTML with some restrictions
    /// </summary>
    SafeHtml = 3
}