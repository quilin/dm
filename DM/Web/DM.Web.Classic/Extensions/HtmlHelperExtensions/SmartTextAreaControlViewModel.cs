using System;
using BBCodeParser.Tags;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public class SmartTextAreaControlViewModel
    {
        public Tag TagInfo { get; set; }
        public Func<IHtmlHelper, IHtmlContent> RenderContent { get; set; }
    }
}