using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Services.Community;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community
{
    /// <inheritdoc />
    [ApiController]
    [Route("v1/reviews")]
    [ApiExplorerSettings(GroupName = "Community")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewApiService reviewApiService;

        /// <inheritdoc />
        public ReviewController(
            IReviewApiService reviewApiService)
        {
            this.reviewApiService = reviewApiService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(CreateReview))]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            var result = await reviewApiService.Create(review);
            return CreatedAtRoute(nameof(GetReview), new {id = result.Resource.Id}, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetReviews))]
        [ProducesResponseType(typeof(ListEnvelope<Review>), 200)]
        public async Task<IActionResult> GetReviews([FromQuery] PagingQuery q) => Ok(await reviewApiService.Get(q));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("random", Name = nameof(GetRandomReview))]
        public async Task<IActionResult> GetRandomReview() => Ok(await reviewApiService.GetRandom());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetReview))]
        public async Task<IActionResult> GetReview(Guid id) => Ok(await reviewApiService.Get(id));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="review"></param>
        /// <returns></returns>
        [HttpPatch("{id}", Name = nameof(PutReview))]
        public async Task<IActionResult> PutReview(Guid id, [FromBody] Review review) =>
            Ok(await reviewApiService.Update(id, review));
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = nameof(DeleteReview))]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            await reviewApiService.Delete(id);
            return NoContent();
        }
    }
}