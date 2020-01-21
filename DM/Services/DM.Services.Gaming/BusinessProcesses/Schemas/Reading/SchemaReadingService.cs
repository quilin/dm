using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
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
        private readonly IIdentity identity;

        /// <inheritdoc />
        public SchemaReadingService(
            ISchemaReadingRepository repository,
            IIdentityProvider identityProvider)
        {
            this.repository = repository;
            identity = identityProvider.Current;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AttributeSchema>> Get() => await repository.GetSchemata(identity.User.UserId);

        /// <inheritdoc />
        public async Task<AttributeSchema> Get(Guid schemaId)
        {
            var attributeSchema = await repository.GetSchema(schemaId);
            if (attributeSchema == null)
            {
                throw new HttpException(HttpStatusCode.Gone, "Schema not found");
            }

            return attributeSchema;
        }
    }
}