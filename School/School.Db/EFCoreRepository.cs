using Alfatraining.Ams.Common.DbRepository.Interfaces;
using Alfatraining.Ams.Common.DbRepository.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Alfatraining.Ams.Common.DbRepository
{
    public class EFCoreRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private DbContext _context;
        private DbSet<TEntity> _dbSet;
        private string _user;

        public EFCoreRepository(DbContext context, string user = "")
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _user = user;
        }
        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }
        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }
        public IEnumerable<TEntity> Take(int count)
        {
            return _dbSet.Take(count).ToList();
        }

        public IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).Take(count).ToList();
        }

        public async Task<IEnumerable<TEntity>> TakeAsync(int count)
        {
            return await _dbSet.Take(count).ToListAsync();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public TEntity FindByIdForReload(int id)
        {
            var item = _dbSet.Find(id);
            if (item != null)
            {
                _context.Entry(item).Reload();
            }

            return item;
        }
        public TEntity Create(TEntity item)
        {
            try
            {
                if (item is IEntity)
                {
                    FillChangeLog(item, "erstellt");
                }
                var itemNew = _dbSet.Add(item).Entity;
                _context.SaveChanges();
                return itemNew;
            }
            catch
            {
                _context.Entry(item).State = EntityState.Detached;
                throw;
            }
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            try
            {
                var itemNew = _dbSet.Add(item).Entity;
                await _context.SaveChangesAsync();

                _context.Entry(item).State = EntityState.Detached;
                //_context.SaveChanges();
                return itemNew;
            }
            catch (Exception ex)
            {
                _context.Entry(item).State = EntityState.Detached;
                throw;
            }
        }

        public TEntity Update(TEntity item, byte[] rowversion, string operation = "")
        {
            try
            {
                if (item is IRowVersion && rowversion != null)
                {
                    _context.Entry(item).OriginalValues["RowVersion"] = rowversion;
                }

                if (item is IEntity)
                {
                    FillChangeLog(item, operation);
                }

                _context.Entry(item).State = EntityState.Modified;
                _context.SaveChanges();

                _context.Entry(item).State = EntityState.Detached;
                //_context.SaveChanges();

                return item;
            }
            catch (Exception)
            {
                _context.Entry(item).State = EntityState.Detached;
                throw;
            }
        }
        public async Task<TEntity> UpdateAsync(TEntity item, byte[] rowversion, string operation = "")
        {
            try
            {
                if (item is IRowVersion && rowversion != null)
                {
                    _context.Entry(item).OriginalValues["RowVersion"] = rowversion;
                }

                if (item is IEntity)
                {
                    FillChangeLog(item, operation);
                }

                _context.Entry(item).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                _context.Entry(item).State = EntityState.Detached;
                //_context.SaveChanges();

                return item;
            }
            catch (Exception)
            {
                _context.Entry(item).State = EntityState.Detached;
                throw;
            }
        }

        public TEntity Update(TEntity item, string operation = "")
        {
            return Update(item, null, operation);
        }

        public async Task<TEntity> UpdateAsync(TEntity item, string operation = "")
        {
            return await UpdateAsync(item, null, operation);
        }
       
        public void Remove(TEntity item)
        {
            _dbSet.Attach(item);
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
        public async Task RemoveAsync(TEntity item)
        {
            _dbSet.Attach(item);
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }
        public void ClearAndRemove(TEntity item)
        {
            _context.ChangeTracker.Clear();
            _context.Remove(item);
            _context.SaveChanges();
        }
        public TEntity Reload(int id)
        {
            var item = _dbSet.Find(id);
            if (item == null)
            {
                return null;
            }
            _context.Entry(item).State = EntityState.Detached;
            var result = _context.Entry(item).GetDatabaseValues();
            if (result == null)
            {
                return null;
            }
            else
            {
                return (TEntity)result.ToObject();
            }
        }
        public async Task<TEntity> ReloadAsync(int id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null)
            {
                return null;
            }
            _context.Entry(item).State = EntityState.Detached;
            var result = await _context.Entry(item).GetDatabaseValuesAsync();
            if (result == null)
            {
                return null;
            }
            else
            {
                return (TEntity)result.ToObject();
            }
        }
        public TEntity ReloadForReload(int id)
        {
            var item = _dbSet.Find(id);
            if (item is null)
            {
                return null;
            }
            _context.Entry(item).Reload();
            _context.Entry(item).State = EntityState.Detached;
            var result = _context.Entry(item).GetDatabaseValues();

            if (result == null)
            {
                return null;
            }
            else
            {
                return (TEntity)result.ToObject();

            }
        }
        private void FillChangeLog(TEntity item, string operation)
        {
            var iEntity = item as IEntity;
            var changeLogJson = string.IsNullOrEmpty(iEntity.ChangeLogJson) ? new List<ChangeLogJson>() : JsonSerializer.Deserialize<List<ChangeLogJson>>(iEntity.ChangeLogJson);
            changeLogJson.Add(new ChangeLogJson()
            {
                Operation = String.IsNullOrEmpty(operation) ? "aktualisiert" : operation,
                User = _user,
                Date = DateTime.Now
            });
            iEntity.ChangeLogJson = JsonSerializer.Serialize(changeLogJson);

            while (iEntity.ChangeLogJson.Length > 4000)
            {
                var firstRecord = changeLogJson.First();
                changeLogJson.Remove(firstRecord);

                iEntity.ChangeLogJson = JsonSerializer.Serialize(changeLogJson);
            }
        }
    }
}
