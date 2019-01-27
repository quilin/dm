namespace Web.Core.Extensions.HtmlHelperExtensions.Dropdown
{
    public class Dropdownable : IDropdownable
    {
        private readonly string description;
        private readonly string additionalData;
        private readonly string value;

        public Dropdownable(string value, string description, string additionalData = null)
        {
            this.value = value;
            this.description = description;
            this.additionalData = additionalData;
        }

        public Dropdownable(string value)
        {
            this.value = value;
            description = value;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetAdditionalData()
        {
            return additionalData;
        }

        public string GetValue()
        {
            return value;
        }
    }
}