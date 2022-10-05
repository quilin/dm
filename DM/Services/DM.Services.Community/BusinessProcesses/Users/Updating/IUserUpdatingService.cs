using System.IO;
using System.Threading.Tasks;
using DM.Services.Community.BusinessProcesses.Users.Reading;

namespace DM.Services.Community.BusinessProcesses.Users.Updating;

/// <summary>
/// Service for user update
/// </summary>
public interface IUserUpdatingService
{
    /// <summary>
    /// Update user details
    /// </summary>
    /// <param name="updateUser"></param>
    /// <returns></returns>
    Task<UserDetails> Update(UpdateUser updateUser);

    /// <summary>
    /// Upload user profile picture
    /// </summary>
    /// <param name="login">User login</param>
    /// <param name="uploadStream">Uploaded file stream</param>
    /// <param name="fileName">File name</param>
    /// <param name="contentType">Content type</param>
    /// <returns></returns>
    Task<UserDetails> UploadPicture(string login, Stream uploadStream, string fileName, string contentType);
}