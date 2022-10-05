using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Community;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Services.Community;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community;

/// <inheritdoc />
[ApiController]
[Route("v1/polls")]
[ApiExplorerSettings(GroupName = "Community")]
public class PollController : ControllerBase
{
    private readonly IPollApiService apiService;

    /// <inheritdoc />
    public PollController(
        IPollApiService apiService)
    {
        this.apiService = apiService;
    }

    /// <summary>
    /// Get list of polls
    /// </summary>
    /// <param name="q"></param>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetPolls))]
    [ProducesResponseType(typeof(ListEnvelope<Poll>), 200)]
    public async Task<IActionResult> GetPolls([FromQuery] PollsQuery q) => Ok(await apiService.Get(q));

    /// <summary>
    /// Create new poll
    /// </summary>
    /// <param name="poll"></param>
    /// <response code="201"></response>
    /// <response code="400">Some poll properties were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to create polls</response>
    [HttpPost(Name = nameof(PostPoll))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Poll>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(BadRequestError), 401)]
    [ProducesResponseType(typeof(BadRequestError), 403)]
    public async Task<IActionResult> PostPoll([FromBody] Poll poll)
    {
        var result = await apiService.Create(poll);
        return CreatedAtRoute(nameof(GetPoll), new {id = result.Resource.Id}, result);
    }

    /// <summary>
    /// Get poll by id
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Poll not found</response>
    [HttpGet("{id}", Name = nameof(GetPoll))]
    [ProducesResponseType(typeof(Envelope<Poll>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetPoll(Guid id) => Ok(await apiService.Get(id));

    /// <summary>
    /// Vote for the poll option
    /// </summary>
    /// <param name="id"></param>
    /// <param name="optionId"></param>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not authorized to vote for this poll</response>
    /// <response code="410">Poll not found</response>
    [HttpPut("{id}", Name = nameof(PutPollVote))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<Poll>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutPollVote(Guid id, [FromQuery] Guid optionId) =>
        Ok(await apiService.Vote(id, optionId));
}