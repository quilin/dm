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
            dbContext.Uploads.Add(upload);
            await dbContext.SaveChangesAsync();
            return await dbContext.Uploads
                .Where(u => u.UploadId == upload.UploadId)
                .ProjectTo<Upload>(mapper.ConfigurationProvider)
                .FirstAsync();
        }
    }
}