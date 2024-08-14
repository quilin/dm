using System.Threading.Tasks;
using DM.Services.Core.Extensions;
using DM.Web.API.Authentication;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using DM.Web.API.Services.Users;
using DM.Web.API.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DM.Web.API.Controllers.v1.Community;

/// <inheritdoc />
[Route("v1/users")]
[ApiExplorerSettings(GroupName = "Community")]
public class UserUploadController : ControllerBase
{
    private readonly IUserApiService userApiService;

    /// <inheritdoc />
    public UserUploadController(
        IUserApiService userApiService)
    {
        this.userApiService = userApiService;
    }

    /// <summary>
    /// Post user profile picture
    /// </summary>
    /// <param name="login"></param>
    /// <param name="file"></param>
    /// <response code="200"></response>
    /// <response code="401">User must be authenticated</response>
    /// <response code="403">User is not allowed to upload profile pictures to this user</response>
    /// <response code="410">User not found</response>
    [HttpPost("{login}/uploads", Name = nameof(PostUserUpload))]
    [AuthenticationRequired]
    [ValidationRequired]
    [ProducesResponseType(typeof(Envelope<UserDetails>), 200)]
    [ProducesResponseType(typeof(GeneralError), 401)]
    [ProducesResponseType(typeof(GeneralError), 403)]
    [ProducesResponseType(typeof(GeneralError), 410)]
    public async Task<IActionResult> PostUserUpload(string login,
        [FileSizeValidation(4 << 20, ErrorMessage = "File must be at most 4Mb big")]
        [FileContentTypeValidation(
            FileMimeTypeNames.Image.Gif,
            FileMimeTypeNames.Image.Jpeg,
            FileMimeTypeNames.Image.Png, ErrorMessage = "File must be a gif/jpg/png image")]
        IFormFile file) =>
        Ok(await userApiService.UploadProfilePicture(login, file));
}