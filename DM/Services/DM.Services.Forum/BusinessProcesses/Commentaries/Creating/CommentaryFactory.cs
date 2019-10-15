using System;
using DM.Services.Core.Implementation;
using DM.Services.DataAccess.BusinessObjects.Common;
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
        public Comment Create(CreateComment createComment, Guid userId)
        {
            return new Comment
            {
                CommentId = guidFactory.Create(),
                EntityId = createComment.TopicId,
                UserId = userId,
                CreateDate = dateTimeProvider.Now,
                Text = createComment.Text.Trim(),
                IsRemoved = false
            };
        }
    }
}