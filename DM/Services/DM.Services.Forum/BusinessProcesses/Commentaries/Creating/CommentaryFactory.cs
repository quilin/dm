using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Fora;
using DM.Services.Forum.Dto.Input;

namespace DM.Services.Forum.BusinessProcesses.Commentaries.Creating
{
    /// <inheritdoc />
    public class CommentaryFactory : ICommentaryFactory
    {
        private readonly IGuidFactory guidFactory;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <inheritdoc />
        public CommentaryFactory(
            IGuidFactory guidFactory,
            IDateTimeProvider dateTimeProvider)
        {
            this.guidFactory = guidFactory;
            this.dateTimeProvider = dateTimeProvider;
        }
        
        /// <inheritdoc />
        public ForumComment Create(CreateComment createComment, Guid userId)
        {
            return new ForumComment
            {
                ForumCommentId = guidFactory.Create(),
                ForumTopicId = createComment.TopicId,
                UserId = userId,
                CreateDate = dateTimeProvider.Now,
                Text = createComment.Text.Trim(),
                IsRemoved = false
            };
        }
    }
}