using System.Threading.Tasks;
using DM.Services.Core.Dto;
using DM.Services.Search.Grpc;
using DM.Web.API.Configuration;
using DM.Web.API.Dto.Contracts;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DM.Web.API.Controllers.v1.Common;

/// <inheritdoc />
[ApiController]
[Route("v1/search")]
[ApiExplorerSettings(GroupName = "Common")]
public class SearchController : ControllerBase
{
    private readonly SearchServiceConfiguration searchServiceConfiguration;

    /// <inheritdoc />
    public SearchController(
        IOptions<SearchServiceConfiguration> options)
    {
        searchServiceConfiguration = options.Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ListEnvelope<object>> Search(
        [FromQuery] string query,
        [FromQuery] PagingQuery q)
    {
        using var channel = GrpcChannel.ForAddress(searchServiceConfiguration.GrpcEndpoint);
        var client = new SearchEngine.SearchEngineClient(channel);
        var searchResponse = await client.SearchAsync(new SearchRequest
        {
            Query = query,
            Skip = q.Skip ?? 0,
            Size = q.Size ?? 10,
        });
        var paging = new Paging(PagingResult.Create(searchResponse.Total, (q.Skip ?? 0) + 1, q.Size ?? 10));
        return new ListEnvelope<object>(searchResponse.Entities, paging);
    }
}