using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Dto;
using DM.Services.Core.Exceptions;

namespace DM.Services.Community.BusinessProcesses.Users.Reading;

/// <inheritdoc />
internal class UserReadingService(
    IIdentityProvider identityProvider,
    IUserReadingRepository readingRepository) : IUserReadingService
{
    /// <inheritdoc />
    public async Task<(IEnumerable<GeneralUser> users, PagingResult paging)> Get(
        PagingQuery query, bool withInactive, CancellationToken cancellationToken)
    {
        var totalCount = await readingRepository.CountUsers(withInactive, cancellationToken);
        var paging = new PagingData(query, identityProvider.Current.Settings.Paging.EntitiesPerPage, totalCount);
        var users = await readingRepository.GetUsers(paging, withInactive, cancellationToken);
        return (users, paging.Result);
    }

    /// <inheritdoc />
    public async Task<GeneralUser> Get(string login, CancellationToken cancellationToken)
    {
        var user = await readingRepository.GetUser(login, cancellationToken);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.Gone, $"User {login} not found");
        }

        return user;
    }

    /// <inheritdoc />
    public async Task<UserDetails> GetDetails(string login, CancellationToken cancellationToken)
    {
        var user = await readingRepository.GetUserDetails(login, cancellationToken);
        if (user == null)
        {
            throw new HttpException(HttpStatusCode.Gone, $"User {login} not found");
        }

        return user;
    }
}