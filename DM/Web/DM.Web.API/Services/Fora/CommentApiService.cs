using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.BusinessProcesses.Commentaries;
using DM.Services.Forum.Dto.Input;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;

namespace DM.Web.API.Services.Fora
{
    /// <inheritdoc />
    public class CommentApiService : ICommentApiService
    {
        private readonly ICommentaryReadingService commentaryReadingService;
        private readonly ICommentaryCreatingService commentaryCreatingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentApiService(
            ICommentaryReadingService commentaryReadingService,
            ICommentaryCreatingService commentaryCreatingService,
            IMapper mapper)
        {
            this.commentaryReadingService = commentaryReadingService;
            this.commentaryCreatingService = commentaryCreatingService;
            this.mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<ListEnvelope<Comment>> Get(Guid topicId, int entityNumber)
        {
            var (comments, paging) = await commentaryReadingService.GetCommentsList(topicId, entityNumber);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }

        /// <inheritdoc />
        public async Task<Envelope<Comment>> Create(Guid topicId, Comment comment)
        {
            var createComment = mapper.Map<CreateComment>(comment);
            createComment.TopicId = topicId;
            var createdComment = await commentaryCreatingService.Create(createComment);
            return new Envelope<Comment>(mapper.Map<Comment>(createdComment));
        }
    }
}