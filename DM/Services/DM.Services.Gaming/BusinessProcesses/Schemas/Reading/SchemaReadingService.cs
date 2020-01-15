using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.Authentication.Dto;
using DM.Services.Authentication.Implementation.UserIdentity;
using DM.Services.Core.Exceptions;
using DM.Services.Gaming.Dto.Shared;

namespace DM.Services.Gaming.BusinessProcesses.Schemas.Reading
{
    /// <inheritdoc />
    public class SchemaReadingService : ISchemaReadingService
    {
        private readonly ISchemaReadingRepository repository;
        private readonly IMapper mapper;
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaReadingService(
            ISchemaReadingRepository repository,
            IMapper mapper,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            this.mapper = mapper;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AttributeSchema>> Get()
        {
            var schemata = (await repository.GetSchemata(identity.User.UserId)).ToArray();

            var userIds = schemata.Where(s => s.UserId.HasValue).Select(s => s.UserId.Value).ToHashSet();
            var schemataAuthors = (await repository.GetSchemataAuthors(userIds)).ToDictionary(u => u.UserId);

            var result = new List<AttributeSchema>(schemata.Length);
            foreach (var schema in schemata)
            {
                var mappedSchema = mapper.Map<AttributeSchema>(schema);
                if (schema.UserId.HasValue && schemataAuthors.TryGetValue(schema.UserId.Value, out var author))
                {
                    mappedSchema.Author = author;
                }

                result.Add(mappedSchema);
            }

            return result;
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Get(Guid schemaId)
        {
            var attributeSchema = await repository.GetSchema(schemaId);
            if (attributeSchema == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Schema not found");
            }

            var schema = mapper.Map<AttributeSchema>(attributeSchema);
            if (attributeSchema.UserId.HasValue)
            {
                var author = (await repository.GetSchemataAuthors(new[] {attributeSchema.UserId.Value}))
                    .FirstOrDefault();
                schema.Author = author;
            }

            return schema;
        }
    }
}