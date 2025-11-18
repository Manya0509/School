using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alfatraining.Ams.Common.DbRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity item);
        Task<TEntity> CreateAsync(TEntity item);
        TEntity FindById(int id);
        Task<TEntity> FindByIdAsync(int id);
        IEnumerable<TEntity> Get();
        Task<IEnumerable<TEntity>> GetAsync();
        IQueryable<TEntity> GetQueryable();
        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);
        IEnumerable<TEntity> Take(int count);
        IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate);
        Task<IEnumerable<TEntity>> TakeAsync(int count);
        void Remove(TEntity item);
        Task RemoveAsync(TEntity item);
        TEntity Update(TEntity item, string operation = "");
        TEntity Reload(int id);
        Task<TEntity> ReloadAsync(int id);
    }
}
