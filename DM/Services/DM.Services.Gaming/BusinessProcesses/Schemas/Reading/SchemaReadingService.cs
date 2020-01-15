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
            var schemata = await repository.GetSchemata(identity.User.UserId);
            return schemata.Select(mapper.Map<AttributeSchema>);
        }

        /// <inheritdoc />
        public async Task<AttributeSchema> Get(Guid schemaId)
        {
            var attributeSchema = await repository.GetSchema(schemaId);
            if (attributeSchema == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Schema not found");
            }

            return mapper.Map<AttributeSchema>(attributeSchema);
        }
    }
}