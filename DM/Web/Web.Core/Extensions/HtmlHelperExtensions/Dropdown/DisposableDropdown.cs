using System;
using System.IO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Core.Extensions.HtmlHelperExtensions.Dropdown
{
    public class DisposableDropdown : IDisposable
    {
        private readonly TextWriter writer;
        private Func<IHtmlContent> End { get; }

        public DisposableDropdown(ViewContext viewContext, Func<IHtmlContent> begin, Func<IHtmlContent> end)
        {
            writer = viewContext.Writer;
            End = end;
            writer.Write(begin());
        }

        public void Dispose()
        {
            writer.Write(End());
        }
    }
}