using Microsoft.AspNetCore.Html;

namespace DM.Web.Classic.Extensions.HtmlHelperExtensions
{
    public class SmartTextAreaViewModel
    {
        public string TextAreaId { get; set; }
        public IHtmlContent TextArea { get; set; }
        public IHtmlContent ValidationMessage { get; set; }
        public SmartTextAreaControlViewModel[] Tags { get; set; }
    }
}