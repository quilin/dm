using System;
using System.Collections.Generic;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// Template rendering exception
    /// </summary>
    public class TemplateRenderException : Exception
    {
        /// <inheritdoc />
        public TemplateRenderException(string templatePath, IEnumerable<string> searchedLocations)
            : base($"Could not locate view {templatePath}, searched locations were: {string.Join(",", searchedLocations)}")
        {
        }
    }
}