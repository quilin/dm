using System;
using System.Threading.Tasks;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using Microsoft.Extensions.Options;
using Nest;

namespace DM.Services.Search.Consumer
{
    /// <inheritdoc />
    public class IndexingRepository : IIndexingRepository
    {
        private readonly IElasticClient client;
        private readonly SearchEngineConfiguration configuration;

        /// <inheritdoc />
        public IndexingRepository(
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
        public Task Delete(Guid entityId)
        {
            return client.DeleteAsync<SearchEntity>(entityId);
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