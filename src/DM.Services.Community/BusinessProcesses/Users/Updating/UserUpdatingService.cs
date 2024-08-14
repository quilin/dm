using System.IO;
using System.Threading.Tasks;
using DM.Services.Common.Authorization;
using DM.Services.Community.BusinessProcesses.Users.Reading;
using DM.Services.DataAccess.BusinessObjects.Users;
using DM.Services.DataAccess.BusinessObjects.Users.Settings;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Uploading.BusinessProcesses.PublicImage;
using DM.Services.Uploading.Dto;
using FluentValidation;

namespace DM.Services.Community.BusinessProcesses.Users.Updating;

/// <inheritdoc />
internal class UserUpdatingService : IUserUpdatingService
{
    private readonly IValidator<UpdateUser> validator;
    private readonly IUserReadingService userReadingService;
    private readonly IPublicImageService imageService;
    private readonly IIntentionManager intentionManager;
    private readonly IUpdateBuilderFactory updateBuilderFactory;
    private readonly IUserUpdatingRepository repository;

    /// <inheritdoc />
    public UserUpdatingService(
        IValidator<UpdateUser> validator,
        IUserReadingService userReadingService,
        IPublicImageService imageService,
        IIntentionManager intentionManager,
        IUpdateBuilderFactory updateBuilderFactory,
        IUserUpdatingRepository repository)
    {
        this.validator = validator;
        this.userReadingService = userReadingService;
        this.imageService = imageService;
        this.intentionManager = intentionManager;
        this.updateBuilderFactory = updateBuilderFactory;
        this.repository = repository;
    }

    /// <inheritdoc />
    public async Task<UserDetails> Update(UpdateUser updateUser)
    {
        await validator.ValidateAndThrowAsync(updateUser);
        var user = await userReadingService.Get(updateUser.Login);
        intentionManager.IsAllowed(UserIntention.Edit, user);

        var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
            .MaybeField(u => u.Status, updateUser.Status?.Trim())
            .MaybeField(u => u.Name, updateUser.Name?.Trim())
            .MaybeField(u => u.Location, updateUser.Location?.Trim())
            .MaybeField(u => u.Skype, updateUser.Skype?.Trim())
            .MaybeField(u => u.Info, updateUser.Info?.Trim())
            .MaybeField(u => u.RatingDisabled, updateUser.RatingDisabled);

        var settingsUpdate = updateBuilderFactory.Create<UserSettings>(user.UserId)
            .MaybeField(u => u.ColorSchema, updateUser.Settings?.ColorSchema)
            .MaybeField(u => u.NannyGreetingsMessage, updateUser.Settings?.NannyGreetingsMessage)
            .MaybeField(u => u.Paging.CommentsPerPage, updateUser.Settings?.Paging?.CommentsPerPage)
            .MaybeField(u => u.Paging.TopicsPerPage, updateUser.Settings?.Paging?.TopicsPerPage)
            .MaybeField(u => u.Paging.MessagesPerPage, updateUser.Settings?.Paging?.MessagesPerPage)
            .MaybeField(u => u.Paging.PostsPerPage, updateUser.Settings?.Paging?.PostsPerPage)
            .MaybeField(u => u.Paging.EntitiesPerPage, updateUser.Settings?.Paging?.EntitiesPerPage);

        await repository.UpdateUser(userUpdate, settingsUpdate);
        return await userReadingService.GetDetails(updateUser.Login);
    }

    /// <inheritdoc />
    public async Task<UserDetails> UploadPicture(string login, Stream uploadStream, string fileName,
        string contentType)
    {
        var user = await userReadingService.Get(login);

        var (original, medium, small) = await imageService.Upload(new CreateUpload
        {
            EntityId = user.UserId,
            FileName = fileName,
            ContentType = contentType,
            StreamAccessor = () => uploadStream
        });

        var userUpdate = updateBuilderFactory.Create<User>(user.UserId)
            .Field(u => u.ProfilePictureUrl, original.FilePath)
            .Field(u => u.MediumProfilePictureUrl, medium.FilePath)
            .Field(u => u.SmallProfilePictureUrl, small.FilePath);
        var settingsUpdate = updateBuilderFactory.Create<UserSettings>(user.UserId);
        await repository.UpdateUser(userUpdate, settingsUpdate);

        await imageService.PrepareObsoleteForDeleting(user.UserId);

        return await userReadingService.GetDetails(login);
    }
}