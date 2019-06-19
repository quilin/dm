using System;
using DM.Services.Core.Parsing;

namespace DM.Web.Classic.Views.Shared.Commentaries
{
    public class CreateCommentaryFormBuilder : ICreateCommentaryFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public CreateCommentaryFormBuilder(
            IBbParserProvider bbParserProvider
            )
        {
            this.bbParserProvider = bbParserProvider;
        }

        public CreateCommentaryForm Build(Guid entityId)
        {
            return new CreateCommentaryForm
            {
                EntityId = entityId,
                Parser = bbParserProvider.CurrentCommon
            };
        }
    }
}