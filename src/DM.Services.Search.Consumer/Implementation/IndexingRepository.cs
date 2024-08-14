using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.DataAccess.SearchEngine;
using DM.Services.Search.Configuration;
using OpenSearch.Client;

namespace DM.Services.Search.Consumer.Implementation;

/// <inheritdoc />
internal class IndexingRepository(IOpenSearchClient client) : IIndexingRepository
{
    public async Task Index(params SearchEntity[] entities)
    {
        await DeclareIndex();
        await client.IndexManyAsync(entities);
    }

    public Task Delete(Guid entityId) => client.DeleteAsync<SearchEntity>(entityId);

    public Task DeleteByParent(Guid parentEntityId) =>
        client.DeleteByQueryAsync<SearchEntity>(d => d.Query(q => q
            .Term(t => t
                .Field(f => f.ParentEntityId)
                .Value(parentEntityId))));

    public Task UpdateByParent(Guid parentEntityId, IEnumerable<UserRole> roles) =>
        client.UpdateByQueryAsync<SearchEntity>(d => d.Query(q => q
                .Term(t => t
                    .Field(f => f.ParentEntityId)
                    .Value(parentEntityId)))
            .Script($"ctx._source.authorizedRoles = [{string.Join(",", roles.Cast<int>())}]"));

    public Task UpdateByParent(Guid parentEntityId, IEnumerable<Guid> userIds) =>
        client.UpdateByQueryAsync<SearchEntity>(d => d.Query(q => q
                .Term(t => t
                    .Field(f => f.ParentEntityId)
                    .Value(parentEntityId)))
            .Script($"ctx._source.authorizedUsers = [{string.Join(",", userIds)}]"));

    private async Task DeclareIndex()
    {
        var existsResponse = await client.Indices.ExistsAsync(SearchEngineConfiguration.IndexName);
        if (existsResponse is { IsValid: true, Exists: true })
        {
            return;
        }

        await client.Indices.CreateAsync(SearchEngineConfiguration.IndexName, i => i
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
            .Map<SearchEntity>(m => m
                .AutoMap()
                .Properties(p => p
                    .Text(t => t
                        .Name(n => n.Text)
                        .Analyzer("dm_analyzer")
                        .SearchAnalyzer("dm_search_analyzer")))));
    }
}