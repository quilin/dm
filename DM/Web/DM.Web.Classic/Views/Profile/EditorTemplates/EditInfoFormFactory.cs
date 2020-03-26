using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Profile.EditorTemplates
{
    public class EditInfoFormFactory : IEditInfoFormFactory
    {
        private readonly IBbParserProvider bbParserProvider;

        public EditInfoFormFactory(
            IBbParserProvider bbParserProvider)
        {
            this.bbParserProvider = bbParserProvider;
        }

        public EditInfoForm Create(string login, string info) => new EditInfoForm
        {
            Login = login,
            Info = info,
            Parser = bbParserProvider.CurrentInfo
        };
    }
}