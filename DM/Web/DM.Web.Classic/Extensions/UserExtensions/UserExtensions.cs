using DM.Web.Classic.Views.Shared.User;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Extensions.UserExtensions
{
    public static class UserExtensions
    {
        public static IHtmlContent UserLink<TModel>(this IHtmlHelper<TModel> html, UserViewModel user,
            bool withBadge = false)
        {
            var profileLinkBuilder = new TagBuilder("a");
            profileLinkBuilder.Attributes.Add("href",
                new UrlHelper(html.ViewContext).Action("Index", "Profile",
                    new RouteValueDictionary {{"login", user.Login}}));
            profileLinkBuilder.AddCssClass("content-userLink");
            profileLinkBuilder.InnerHtml.Append(user.Login);
            var resultBuilder = new HtmlContentBuilder()
                .AppendHtml(profileLinkBuilder);
            
            if (withBadge && (user.IsAdministrator || user.IsModerator))
            {
                var badgeWrapperBuilder = new TagBuilder("span");
                badgeWrapperBuilder.AddCssClass("content-userLink-badge-wrapper");
                var badgeContent = new HtmlContentBuilder().Append("[");
                var badgeBuilder = new TagBuilder("span");
                badgeBuilder.AddCssClass("content-userLink-badge");
                badgeBuilder.InnerHtml.Append(user.IsAdministrator ? "A" : "M");
                badgeContent.AppendHtml(badgeBuilder);
                badgeContent.Append("]");
                badgeWrapperBuilder.InnerHtml.AppendHtml(badgeContent);

                resultBuilder
                    .AppendHtml("&nbsp;")
                    .AppendHtml(badgeWrapperBuilder);
            }

            return resultBuilder;
        }
    }
}