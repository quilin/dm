using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using DbUpload = DM.Services.DataAccess.BusinessObjects.Common.Upload;

namespace DM.Services.Common.BusinessProcesses.Uploads
{
    /// <inheritdoc />
    public class UploadRepository : IUploadRepository
    {
        private readonly DmDbContext dbContext;
        private readonly IMapper mapper;

        /// <inheritdoc />
        public UploadRepository(
            DmDbContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }
        
        /// <inheritdoc />
        public async Task<Upload> Create(DbUpload upload)
        {
            await dbContext.Uploads.AddAsync(upload);
            await dbContext.SaveChangesAsync();
            return await dbContext.Uploads
                .Where(u => u.UploadId == upload.UploadId)
                .ProjectTo<Upload>(mapper.ConfigurationProvider)
                .FirstAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Upload>> Create(IEnumerable<DbUpload> uploads)
        {
            var savingUploads = uploads.ToArray();
            await dbContext.Uploads.AddRangeAsync(savingUploads);
            var uploadIds = savingUploads.Select(upload => upload.UploadId);
            await dbContext.SaveChangesAsync();
            return await dbContext.Uploads
                .Where(u => uploadIds.Contains(u.UploadId))
                .ProjectTo<Upload>(mapper.ConfigurationProvider)
                .ToArrayAsync();
        }
    }
}