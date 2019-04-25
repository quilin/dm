using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Core.Dto;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.Dto.Input;
using DM.Web.API.Dto.Contracts;
using Comment = DM.Web.API.Dto.Fora.Comment;

namespace DM.Web.API.Services.Fora
{
    /// <inheritdoc />
    public class CommentApiService : ICommentApiService
    {
        private readonly ICommentaryReadingService readingService;
        private readonly ICommentaryCreatingService creatingService;
        private readonly ICommentaryUpdatingService updatingService;
        private readonly ICommentaryDeletingService deletingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentApiService(
            ICommentaryReadingService readingService,
            ICommentaryCreatingService creatingService,
            ICommentaryUpdatingService updatingService,
            ICommentaryDeletingService deletingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.creatingService = creatingService;
            this.updatingService = updatingService;
            this.deletingService = deletingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Comment>> Get(Guid topicId, PagingQuery query)
        {
            var (comments, paging) = await readingService.GetCommentsList(topicId, query);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<Comment>> Create(Guid topicId, Comment comment)
        {
            var createComment = mapper.Map<CreateComment>(comment);
            createComment.TopicId = topicId;
            var createdComment = await creatingService.Create(createComment);
            return new Envelope<Comment>(mapper.Map<Comment>(createdComment));
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