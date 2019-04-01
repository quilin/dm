using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.SearchEngine.Configuration;
using DM.Services.SearchEngine.Dto;
using Microsoft.Extensions.Options;
using Nest;

namespace DM.Services.SearchEngine.Repositories
{
    /// <inheritdoc />
    public class SearchEngineRepository : ISearchEngineRepository
    {
        private readonly IElasticClient client;
        private readonly SearchEngineConfiguration configuration;

        /// <inheritdoc />
        public SearchEngineRepository(
            IElasticClient client,
            IOptions<SearchEngineConfiguration> options)
        {
            this.client = client;
            configuration = options.Value;
        }

        /// <inheritdoc />
        public async Task Index(params SearchEntity[] entities)
        {
            await DeclareIndex();
            await client.IndexManyAsync(entities);
        }

        /// <inheritdoc />
        public Task<(IEnumerable<FoundEntity> entities, int totalCount)> Search(string query, int skip, int take,
            UserRole role, Guid userId)
        {
            throw new NotImplementedException();
        }

        private async Task DeclareIndex()
        {
            if ((await client.IndexExistsAsync(configuration.IndexName)).Exists)
            {
                return;
            }

            await client.CreateIndexAsync(configuration.IndexName, i => i
                .Settings(s => s
                    .Analysis(a => a
                        .Analyzers(an => an
                            .Custom("dm_analyzer", ca => ca
                                .CharFilters("html_strip")
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase", "stop"))
                            .Custom("dm_search_analyzer", ca => ca
                                .Tokenizer("standard")
                                .Filters("standard", "lowercase", "stop")))))
                .Mappings(m => m.Map<SearchEntity>(mm => mm
                    .AutoMap()
                    .Properties(p => p
                        .Text(t => t
                            .Name(n => n.Text)
                            .Analyzer("dm_analyzer")
                            .SearchAnalyzer("dm_search_analyzer"))))));
        }
    }
}