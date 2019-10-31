using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Core.Dto;
using DM.Services.Gaming.BusinessProcesses.Commentaries.Reading;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Shared;

namespace DM.Web.API.Services.Gaming
{
    /// <inheritdoc />
    public class CommentApiService : ICommentApiService
    {
        private readonly ICommentaryReadingService readingService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public CommentApiService(
            ICommentaryReadingService readingService,
            IMapper mapper)
        {
            this.readingService = readingService;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<ListEnvelope<Comment>> Get(Guid gameId, PagingQuery query)
        {
            var (comments, paging) = await readingService.Get(gameId, query);
            return new ListEnvelope<Comment>(comments.Select(mapper.Map<Comment>), new Paging(paging));
        }

        /// <inheritdoc />
        public Task<Envelope<Comment>> Create(Guid gameId, Comment comment)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public async Task<Envelope<Comment>> Get(Guid commentId)
        {
            var comment = await readingService.Get(commentId);
            return new Envelope<Comment>(mapper.Map<Comment>(comment));
        }

        /// <inheritdoc />
        public Task<Envelope<Comment>> Update(Guid commentId, Comment comment)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Task Delete(Guid commentId)
        {
            throw new NotImplementedException();
        }
    }
}