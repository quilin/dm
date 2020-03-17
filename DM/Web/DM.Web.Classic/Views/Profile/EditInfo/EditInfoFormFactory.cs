using System;
using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Profile.EditInfo
{
    public class EditInfoFormFactory : IEditInfoFormFactory
    {
        private readonly IBbParserProvider bbParserProvider;

        public EditInfoFormFactory(
            IBbParserProvider bbParserProvider)
        {
            this.bbParserProvider = bbParserProvider;
        }

        public EditInfoForm Create(Guid userId, string info)
        {
            return new EditInfoForm
            {
                UserId = userId,
                Info = info,
                Parser = bbParserProvider.CurrentInfo
            };
        }
    }
}