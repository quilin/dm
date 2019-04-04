using System;
using System.Threading.Tasks;
using DM.Web.API.Dto.Common;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Fora;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Fora;
using DM.Web.Core.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Fora
{
    /// <summary>
    /// Topics
    /// </summary>
    [Route("v1/topics")]
    public class TopicController : Controller
    {
        private readonly ITopicApiService topicApiService;
        private readonly ILikeApiService likeApiService;

        /// <inheritdoc />
        public TopicController(
            ITopicApiService topicApiService,
            ILikeApiService likeApiService)
        {
            this.topicApiService = topicApiService;
            this.likeApiService = likeApiService;
        }

        /// <summary>
        /// Get certain topic
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="404">No topic was found for passed id</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> GetTopic(Guid id) => topicApiService.Get(id);

        /// <summary>
        /// Put topic changes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topic">Topic</param>
        /// <response code="200"></response>
        /// <response code="400">Some of topic changed properties were invalid or passed id was not recognized</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not authorized to change some properties of this topic</response>
        /// <response code="404">No topic was found for passed id</response>
        [HttpPut("{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Topic>), 200)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Topic>> PutTopic(Guid id, [FromBody] Topic topic) => topicApiService.Update(id, topic);

        /// <summary>
        /// Delete certain topic
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove the topic</response>
        /// <response code="404">No topic was found for passed id</response>
        [HttpDelete("{id}")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task DeleteTopic(Guid id) => topicApiService.Delete(id);

        /// <summary>
        /// Post new like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="201"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to like the topic</response>
        /// <response code="404">No topic was found for passed id</response>
        /// <response code="409">User already liked this topic</response>
        [HttpPost("{id}/likes")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<User>), 201)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task<Envelope<User>> PostLike(Guid id) => likeApiService.LikeTopic(id);

        /// <summary>
        /// Delete like
        /// </summary>
        /// <param name="id"></param>
        /// <response code="204"></response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to remove like from this topic</response>
        /// <response code="404">No topic was found for passed id</response>
        /// <response code="409">User has no like for this topic</response>
        [HttpDelete("{id}/likes")]
        [AuthenticationRequired]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        [ProducesResponseType(typeof(GeneralError), 409)]
        public Task DeleteLike(Guid id) => likeApiService.DislikeTopic(id);

        /// <summary>
        /// Get list of comments
        /// </summary>
        /// <param name="id"></param>
        /// <param name="n">Entity number</param>
        /// <response code="200"></response>
        /// <response code="404">No topic was found for passed id</response>
        [HttpGet("{id}/comments")]
        [ProducesResponseType(typeof(ListEnvelope<Topic>), 200)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<ListEnvelope<Comment>> GetComments(Guid id, [FromQuery] int n = 1) => topicApiService.Get(id, n);

        /// <summary>
        /// Post new comment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comment">Comment</param>
        /// <response code="201"></response>
        /// <response code="400">Some of comment properties were invalid</response>
        /// <response code="401">User must be authenticated</response>
        /// <response code="403">User is not allowed to comment this topic</response>
        /// <response code="404">No topic was found for passed id</response>
        [HttpPost("{id}/comments")]
        [AuthenticationRequired]
        [ProducesResponseType(typeof(Envelope<Comment>), 201)]
        [ProducesResponseType(typeof(BadRequestError), 400)]
        [ProducesResponseType(typeof(GeneralError), 401)]
        [ProducesResponseType(typeof(GeneralError), 403)]
        [ProducesResponseType(typeof(GeneralError), 404)]
        public Task<Envelope<Comment>> PostComment(Guid id, [FromBody] Comment comment) =>
            throw new NotImplementedException();
    }
}