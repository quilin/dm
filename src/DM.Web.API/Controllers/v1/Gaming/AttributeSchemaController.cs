using System;
using System.Threading.Tasks;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Games.Attributes;
using DM.Web.API.Services.Gaming;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Gaming;

/// <inheritdoc />
[ApiController]
[Route("v1/schemata")]
[ApiExplorerSettings(GroupName = "Game")]
public class AttributeSchemaController : ControllerBase
{
    private readonly ISchemaApiService schemaApiService;

    /// <inheritdoc />
    public AttributeSchemaController(
        ISchemaApiService schemaApiService)
    {
        this.schemaApiService = schemaApiService;
    }

    /// <summary>
    /// Get list of game attribute schemas
    /// </summary>
    /// <response code="200"></response>
    [HttpGet(Name = nameof(GetSchemas))]
    [ProducesResponseType(typeof(ListEnvelope<AttributeSchema>), 200)]
    public async Task<IActionResult> GetSchemas() => Ok(await schemaApiService.Get());

    /// <summary>
    /// Post new attribute schema
    /// </summary>
    /// <param name="schema"></param>
    /// <response code="201"></response>
    /// <response code="400">Some of schema parameters were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to create attribute schemas</response>
    [HttpPost(Name = nameof(PostSchema))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<AttributeSchema>), 201)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    public async Task<IActionResult> PostSchema([FromBody] AttributeSchema schema)
    {
        var result = await schemaApiService.Create(schema);
        return CreatedAtRoute(nameof(GetSchema), new {id = result.Resource.Id}, result);
    } 

    /// <summary>
    /// Get certain attribute schema
    /// </summary>
    /// <param name="id"></param>
    /// <response code="200"></response>
    /// <response code="410">Schema not found</response>
    [HttpGet("{id}", Name = nameof(GetSchema))]
    [ProducesResponseType(typeof(Envelope<AttributeSchema>), 200)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> GetSchema(Guid id) => Ok(await schemaApiService.Get(id));

    /// <summary>
    /// Update existing attribute schema
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schema"></param>
    /// <response code="200"></response>
    /// <response code="400">Some of schema parameters were invalid</response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to update this attribute schema</response>
    /// <response code="410">Schema not found</response>
    [HttpPatch("{id}", Name = nameof(PutSchema))]
    [AuthenticationRequired]
    [ProducesResponseType(typeof(Envelope<AttributeSchema>), 200)]
    [ProducesResponseType(typeof(BadRequestError), 400)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PutSchema(Guid id, [FromBody] AttributeSchema schema) =>
        Ok(await schemaApiService.Update(id, schema));

    /// <summary>
    /// Delete existing attribute schema
    /// </summary>
    /// <param name="id"></param>
    /// <response code="204"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to delete this attribute schema</response>
    /// <response code="410">Schema not found</response>
    [HttpDelete("{id}", Name = nameof(DeleteSchema))]
    [AuthenticationRequired]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> DeleteSchema(Guid id)
    {
        await schemaApiService.Delete(id);
        return NoContent();
    }
}