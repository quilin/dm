using DM.Services.Core.Dto;
using DM.Web.Classic.Views.Shared.User;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;

namespace DM.Web.Classic.Extensions.UserExtensions
{
    public static class UserRatingExtensions
    {
        private static IHtmlContent Format(this GeneralUser user)
        {
            var tagBuilder = new TagBuilder("span");
            tagBuilder.AddCssClass("rating-value");
            if (user.QualityRating != 0)
            {
                tagBuilder.AddCssClass(user.QualityRating > 0 ? "rating-positive" : "rating-negative");
            }
            tagBuilder.InnerHtml.Append(user.QualityRating.ToString());

            return new HtmlContentBuilder()
                .AppendHtml(tagBuilder)
                .Append($"/{user.QuantityRating}");
        }

        public static IHtmlContent RatingLink<TModel>(this IHtmlHelper<TModel> htmlHelper, UserViewModel user)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext);

            var tagBuilder = new TagBuilder("a");
            tagBuilder.AddCssClass("rating");
            tagBuilder.Attributes.Add("href", urlHelper.Action("UserRating", "Votes", new RouteValueDictionary{{"login", user.Login}}));

            tagBuilder.InnerHtml.AppendHtml(user.Rating.Disabled
                ? new HtmlString("n/a")
                : user.Rating.Format());

            return tagBuilder;
        }
    }
}