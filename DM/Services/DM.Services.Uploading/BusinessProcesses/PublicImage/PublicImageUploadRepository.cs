using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using DM.Services.DataAccess.RelationalStorage;
using DM.Services.Uploading.Dto;
using Microsoft.EntityFrameworkCore;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Uploading.BusinessProcesses.PublicImage;

/// <inheritdoc />
internal class PublicImageUploadRepository : IPublicImageUploadRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;
    private readonly IUpdateBuilderFactory updateBuilderFactory;

    /// <inheritdoc />
    public PublicImageUploadRepository(
        DmDbContext dbContext,
        IMapper mapper,
        IUpdateBuilderFactory updateBuilderFactory)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.updateBuilderFactory = updateBuilderFactory;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Upload>> Create(IEnumerable<DbUpload> uploads)
    {
        var savingUploads = uploads.ToArray();
        var uploadIds = savingUploads.Select(u => u.UploadId).ToHashSet();
        await dbContext.Uploads.AddRangeAsync(savingUploads);
        await dbContext.SaveChangesAsync();
        return await dbContext.Uploads
            .Where(u => uploadIds.Contains(u.UploadId))
            .ProjectTo<Upload>(mapper.ConfigurationProvider)
            .ToArrayAsync();
    }

    /// <inheritdoc />
    public async Task RemoveObsoleteUploads(Guid entityId)
    {
        var uploadsInfo = await dbContext.Uploads
            .Where(u => !u.IsRemoved)
            .Where(u => u.EntityId == entityId)
            .OrderByDescending(u => u.CreateDate)
            .Select(u => new {u.CreateDate, u.UploadId})
            .ToArrayAsync();

        var obsoleteUploadUpdates = uploadsInfo
            .GroupBy(u => u.CreateDate)
            .OrderByDescending(g => g.Key)
            .Skip(1)
            .SelectMany(g => g.Select(u => u.UploadId))
            .Select(id => updateBuilderFactory.Create<DbUpload>(id).Field(u => u.IsRemoved, true));

        foreach (var uploadUpdate in obsoleteUploadUpdates)
        {
            uploadUpdate.AttachTo(dbContext);
        }

        await dbContext.SaveChangesAsync();
    }
}