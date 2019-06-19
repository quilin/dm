using System;

namespace DM.Web.Classic.Extensions
{
    public class FormFieldLinkAttribute : Attribute
    {
        public string FieldName { get; set; }
    }
}