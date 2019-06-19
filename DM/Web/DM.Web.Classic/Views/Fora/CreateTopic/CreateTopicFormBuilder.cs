using DM.Services.Core.Parsing;
using DM.Services.Forum.Dto.Output;

namespace DM.Web.Classic.Views.Fora.CreateTopic
{
    public class CreateTopicFormBuilder : ICreateTopicFormBuilder
    {
        private readonly IBbParserProvider bbParserProvider;

        public CreateTopicFormBuilder(
            IBbParserProvider bbParserProvider)
        {
            this.bbParserProvider = bbParserProvider;
        }

        public CreateTopicForm Build(Forum forum)
        {
            return new CreateTopicForm
            {
                ForumId = forum.Title,
                Parser = bbParserProvider.CurrentCommon
            };
        }
    }
}