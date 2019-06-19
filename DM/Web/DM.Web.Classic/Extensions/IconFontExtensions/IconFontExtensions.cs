using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.IconFontExtensions
{
    public static class IconFontExtensions
    {
        public static IHtmlContent UnreadCommentsIcon<TModel>(this IHtmlHelper<TModel> htmlHelper,
            int unreadCommentsCount, bool hideIfNone)
        {
            if (hideIfNone && unreadCommentsCount == 0)
            {
                return new HtmlString(string.Empty);
            }

            if (unreadCommentsCount == 0)
            {
                return IconFont.Render(IconType.NoUnreadComment);
            }

            return new HtmlContentBuilder()
                .AppendHtml(IconFont.Render(IconType.UnreadComment))
                .AppendHtml("&nbsp;")
                .Append(unreadCommentsCount.ToString());
        }

        public static IHtmlContent UnreadPostsIcon<TModel>(this IHtmlHelper<TModel> htmlHelper, int unreadPostsCount,
            bool hideIfNone)
        {
            if (hideIfNone && unreadPostsCount == 0)
            {
                return new HtmlString(string.Empty);
            }

            var icon = IconFont.Render(unreadPostsCount > 0 ? IconType.UnreadPost : IconType.NoUnreadPost);
            return new HtmlContentBuilder()
                .AppendHtml(icon)
                .Append(unreadPostsCount > 0 ? unreadPostsCount.ToString() : string.Empty);
        }

        public static IHtmlContent UnreadMessagesIcon<TModel>(this IHtmlHelper<TModel> htmlHelper,
            int unreadMessagesCount)
        {
            var icon = IconFont.Render(unreadMessagesCount > 0 ? IconType.UnreadMessage : IconType.NoUnreadMessage);

            var result = new HtmlContentBuilder().AppendHtml(icon);
            if (unreadMessagesCount == 0)
            {
                return result;
            }

            return result
                .AppendHtml("&nbsp;")
                .Append(unreadMessagesCount.ToString());
        }
    }
}