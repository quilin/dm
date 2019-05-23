using System;
using System.Collections.Generic;

namespace DM.Services.Core.Rendering
{
    /// <summary>
    /// No template found for 
    /// </summary>
    public class TemplateNotFoundException : Exception
    {
        /// <inheritdoc />
        public TemplateNotFoundException(string templateName, IEnumerable<string> searchLocations)
            : base($"Template {templateName} not found. Searched locations are: {string.Join(Environment.NewLine, searchLocations)}")
        {
        }
    }
}