using Nest;
using System;
using Box.Elastic.Repository.Models;

namespace Box.Elastic.Repository.Context;

public class ElasticDbContext
{
    public IElasticClient Client { get; }

    public ElasticDbContext(string uri)
    {
        var settings = new ConnectionSettings(new Uri(uri));
        Client = new ElasticClient(settings);
    }

    public ElasticSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
    {
        var indexName = typeof(TEntity).Name.ToLower();
        return new ElasticSet<TEntity>(Client, indexName);
    }
}
