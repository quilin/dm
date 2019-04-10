using System;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Common.BusinessProcesses.Commentaries;
using DM.Services.Common.Dto;
using DM.Web.API.Dto.Contracts;
using Comment = DM.Web.API.Dto.Common.Comment;

namespace DM.Web.API.Services.Common
{
    /// <inheritdoc />
    public class CommentApiService : ICommentApiService
    {
        private readonly ICommentaryReadingService readingService;
        private readonly ICommentaryUpdatingService updatingService;
        private readonly ICommentaryDeletingService deletingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentApiService(
            ICommentaryReadingService readingService,
            ICommentaryUpdatingService updatingService,
            ICommentaryDeletingService deletingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.updatingService = updatingService;
            this.deletingService = deletingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<Envelope<Comment>> Get(Guid commentId)
        {
            var comment = await readingService.Get(commentId);
            return new Envelope<Comment>(mapper.Map<Comment>(comment));
        }

        /// <inheritdoc />
        public async Task<Envelope<Comment>> Update(Guid commentId, Comment comment)
        {
            var updatedComment = await updatingService.Update(mapper.Map<UpdateComment>(comment));
            return new Envelope<Comment>(mapper.Map<Comment>(updatedComment));
        }

        /// <inheritdoc />
        public Task Delete(Guid commentId) => deletingService.Delete(commentId);
    }
}