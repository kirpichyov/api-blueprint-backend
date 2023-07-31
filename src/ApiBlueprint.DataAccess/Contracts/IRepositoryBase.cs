using System.Collections.Generic;

namespace ApiBlueprint.DataAccess.Contracts;

public interface IRepositoryBase<in TEntity> 
    where TEntity : class
{
    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
}
