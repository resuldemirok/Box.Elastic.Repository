using System;
using Nest;
using System.Linq;
using System.Linq.Expressions;
using Box.Elastic.Repository.Models;

namespace Box.Elastic.Repository.Context;

public class ElasticSet<TEntity> where TEntity : BaseEntity
{
    private readonly IElasticClient _client;
    private readonly string _indexName;

    public ElasticSet(IElasticClient client, string indexName)
    {
        _client = client;
        _indexName = indexName;

        if (!_client.Indices.Exists(_indexName).Exists)
            _client.Indices.Create(_indexName, c => c.Map<TEntity>(m => m.AutoMap()));
    }

    public IQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate = null)
    {
        var searchResponse = _client.Search<TEntity>(s =>
        {
            s.Index(_indexName).Size(1000);

            if (predicate != null)
                return s.Query(q => q.MatchAll());

            return s.MatchAll();
        });

        return searchResponse.Documents.AsQueryable();
    }

    public void Add(TEntity entity)
    {
        _client.Index(entity, i => i.Index(_indexName).Id(entity.Id));
        RefreshIndex();
    }

    public void Update(TEntity entity)
    {
        _client.Index(entity, i => i.Index(_indexName).Id(entity.Id));
        RefreshIndex();
    }

    public void Delete(string id)
    {
        _client.Delete<TEntity>(id, d => d.Index(_indexName));
        RefreshIndex();
    }

    private void RefreshIndex()
    {
        _client.Indices.Refresh(_indexName);
    }
}
