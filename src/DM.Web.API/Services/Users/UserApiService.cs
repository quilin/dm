using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.Community.BusinessProcesses.Users.Updating;
using DM.Web.API.Dto.Contracts;
using DM.Web.API.Dto.Users;
using Microsoft.AspNetCore.Http;
using UserDetails = DM.Web.API.Dto.Users.UserDetails;

namespace DM.Web.API.Services.Users;

/// <inheritdoc />
internal class UserApiService(
    IUserReadingService readingService,
    IUserUpdatingService updatingService,
    IMapper mapper) : IUserApiService
{
    /// <inheritdoc />
    public async Task<ListEnvelope<User>> GetUsers(UsersQuery query, CancellationToken cancellationToken)
    {
        var (users, paging) = await readingService.Get(query, query.Inactive, cancellationToken);
        return new ListEnvelope<User>(users.Select(mapper.Map<User>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> GetUser(string login, CancellationToken cancellationToken)
    {
        var user = await readingService.Get(login, cancellationToken);
        return new Envelope<User>(mapper.Map<User>(user));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> GetUserDetails(string login, CancellationToken cancellationToken)
    {
        var user = await readingService.GetDetails(login, cancellationToken);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(user));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> UpdateUser(
        string login, UserDetails user, CancellationToken cancellationToken)
    {
        var updateUser = mapper.Map<UpdateUser>(user);
        updateUser.Login = login;
        var updatedUser = await updatingService.Update(updateUser, cancellationToken);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(updatedUser));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> UploadProfilePicture(
        string login, IFormFile file, CancellationToken cancellationToken)
    {
        await using var uploadStream = file.OpenReadStream();
        var updatedUser = await updatingService.UploadPicture(
            login, uploadStream, file.Name, file.ContentType, cancellationToken);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(updatedUser));
    }
}