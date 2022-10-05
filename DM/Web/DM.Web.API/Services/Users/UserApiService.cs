using System.Linq;
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
internal class UserApiService : IUserApiService
{
    private readonly IUserReadingService readingService;
    private readonly IUserUpdatingService updatingService;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public UserApiService(
        IUserReadingService readingService,
        IUserUpdatingService updatingService,
        IMapper mapper)
    {
        this.readingService = readingService;
        this.updatingService = updatingService;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ListEnvelope<User>> GetUsers(UsersQuery query)
    {
        var (users, paging) = await readingService.Get(query, query.Inactive);
        return new ListEnvelope<User>(users.Select(mapper.Map<User>), new Paging(paging));
    }

    /// <inheritdoc />
    public async Task<Envelope<User>> GetUser(string login)
    {
        var user = await readingService.Get(login);
        return new Envelope<User>(mapper.Map<User>(user));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> GetUserDetails(string login)
    {
        var user = await readingService.GetDetails(login);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(user));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> UpdateUser(string login, UserDetails user)
    {
        var updateUser = mapper.Map<UpdateUser>(user);
        updateUser.Login = login;
        var updatedUser = await updatingService.Update(updateUser);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(updatedUser));
    }

    /// <inheritdoc />
    public async Task<Envelope<UserDetails>> UploadProfilePicture(string login, IFormFile file)
    {
        await using var uploadStream = file.OpenReadStream();
        var updatedUser = await updatingService.UploadPicture(login, uploadStream, file.Name, file.ContentType);
        return new Envelope<UserDetails>(mapper.Map<UserDetails>(updatedUser));
    }
}