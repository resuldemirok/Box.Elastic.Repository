using System;
using System.Linq;
using Box.Elastic.Repository.Models;
using System.Linq.Expressions;
using Box.Elastic.Repository.Context;

namespace Box.Elastic.Repository.Repository;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
    void Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(string id);
}

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ElasticSet<TEntity> _set;

    public GenericRepository(ElasticDbContext context)
    {
        _set = context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll() => _set.AsQueryable();

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        => _set.AsQueryable(predicate);

    public void Add(TEntity entity) => _set.Add(entity);
    public void Update(TEntity entity) => _set.Update(entity);
    public void Delete(string id) => _set.Delete(id);
}