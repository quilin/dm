using System;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Forum.Implementation;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;

namespace DM.Web.API.Services.Fora
{
    /// <inheritdoc />
    public class LikeApiService : ILikeApiService
    {
        private readonly IForumService forumService;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public LikeApiService(
            IForumService forumService,
            IMapper mapper)
        {
            this.forumService = forumService;
            this.mapper = mapper;
        }
    
        /// <inheritdoc />
        public async Task<Envelope<User>> LikeTopic(Guid topicId)
        {
            var likedByUser = await forumService.LikeTopic(topicId);
            return new Envelope<User>(mapper.Map<User>(likedByUser));
        }

        /// <inheritdoc />
        public Task DislikeTopic(Guid topicId) => forumService.DislikeTopic(topicId);
    }
}