using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DM.Services.Core.Dto;
using DM.Services.DataAccess;
using DM.Services.DataAccess.MongoIntegration;
using DM.Services.Gaming.Dto.Shared;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using DbAttributeSchema = DM.Services.DataAccess.BusinessObjects.Games.Characters.Attributes.AttributeSchema;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Creating;

/// <inheritdoc />
internal class SchemaCreatingRepository : MongoCollectionRepository<DbAttributeSchema>, ISchemaCreatingRepository
{
    private readonly DmDbContext dbContext;
    private readonly IMapper mapper;

    /// <inheritdoc />
    public SchemaCreatingRepository(
        DmMongoClient client,
        DmDbContext dbContext,
        IMapper mapper) : base(client)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<AttributeSchema> Create(DbAttributeSchema schema)
    {
        await Collection.InsertOneAsync(schema);
        var createdSchema = await Collection
            .Find(Filter.Eq(s => s.Id, schema.Id))
            .FirstAsync();

        var result = mapper.Map<AttributeSchema>(createdSchema);
        if (createdSchema.UserId.HasValue)
        {
            var author = await dbContext.Users
                .Where(u => u.UserId == createdSchema.UserId.Value)
                .ProjectTo<GeneralUser>(mapper.ConfigurationProvider)
                .FirstAsync();
            result.Author = author;
        }

        return result;
    }
}