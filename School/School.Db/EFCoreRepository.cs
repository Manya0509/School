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
        public IEnumerable<TEntity> Get() //Синхронное получение всех данных(запрос выполняется сразу, загружает все записи в память,
                                          //объекты не отслеживаются для изменений,блокирует поток) - когда нужны все записи и они не будут изменяться
        {
            return _dbSet.AsNoTracking().ToList(); //AsNoTracking - отключает отслеживание изменений сущностей.
        }

        public async Task<IEnumerable<TEntity>> GetAsync() //Асинхронное получение всех данных(не блокирует поток, загружает все записи в память,
                                                           //объекты не отслеживаются) - когда нужны все записи и они не будут изменяться
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IQueryable<TEntity> GetQueryable() //Отсоединенный запрос (NoTracking) - ( запрос выполняется при итерации, объекты не отслеживаются,
                                                  //можно строить сложные запросы) - для сложных запросов на чтение (отчеты, фильтрация)
        {
            return _dbSet.AsNoTracking().AsQueryable();
        }
        public IQueryable<TEntity> GetQuery() //Отслеживаемый запрос (Tracking) - (объекты отслеживаются для изменений, возможность изменений)
                                              // - когда планируете изменять полученные данные
        {
            return _dbSet.AsQueryable();
        }
        public IEnumerable<TEntity> Take(int count) //Ограничение количества записей(Получить первые N записей без фильтрации)
        {
            return _dbSet.Take(count).ToList();
        }

        public IEnumerable<TEntity> Take(int count, Func<TEntity, bool> predicate) //Фильтрация происходит в памяти, а не в БД
                                                                                   //(var adultStudents = repository.Take(3, s => s.Age >= 18);)
        {
            return _dbSet.Where(predicate).Take(count).ToList();
        }

        public async Task<IEnumerable<TEntity>> TakeAsync(int count) // Асинхронная версия
        {
            return await _dbSet.Take(count).ToListAsync();
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate) //Загружает все данные из БД, потом фильтрует в памяти
        {
            return _dbSet.Where(predicate).ToList();
        }

        public TEntity FindById(int id) //Поиск по первичному ключу(Сначала проверяет кэш контекста,
                                        //Не выполняет SQL если объект уже загружен, Отслеживает изменения(Tracking))
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> FindByIdAsync(int id) // Асинхронная версия
        {
            return await _dbSet.FindAsync(id);
        }
        public TEntity FindByIdForReload(int id) //Принудительная перезагрузка(Обновить сущность из БД, игнорируя локальные изменения)
        {
            var item = _dbSet.Find(id);
            if (item != null)
            {
                _context.Entry(item).Reload();
            }

            return item;
        }
        public TEntity Create(TEntity item) //Создает новую сущность в базе данных с заполнением журнала изменений.
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

        public async Task<TEntity> CreateAsync(TEntity item) //Асинхронно создает новую сущность в базе данных без журнала изменений.
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

        public TEntity Update(TEntity item, byte[] rowversion, string operation = "") //Обновляет сущность с контролем версий и записью в журнал изменений.
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
        public async Task<TEntity> UpdateAsync(TEntity item, byte[] rowversion, string operation = "") //Асинхронно обновляет сущность с контролем версий и журналом изменений.
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

        public TEntity Update(TEntity item, string operation = "") //Обновляет сущность без контроля версий, но с журналом изменений.
        {
            return Update(item, null, operation);
        }

        public async Task<TEntity> UpdateAsync(TEntity item, string operation = "") //Асинхронно обновляет сущность без контроля версий, но с журналом изменений.
        {
            return await UpdateAsync(item, null, operation);
        }
       
        public void Remove(TEntity item) //Удаляет сущность из базы данных с предварительным присоединением к контексту.
        {
            _dbSet.Attach(item);
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
        public async Task RemoveAsync(TEntity item) //Асинхронно удаляет сущность из базы данных с предварительным присоединением.
        {
            _dbSet.Attach(item);
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }
        public void ClearAndRemove(TEntity item) //Очищает трекер изменений и полностью удаляет сущность из базы данных.
        {
            _context.ChangeTracker.Clear();
            _context.Remove(item);
            _context.SaveChanges();
        }
        public TEntity Reload(int id) //Перезагружает сущность из базы данных в отсоединенном состоянии.
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
        public async Task<TEntity> ReloadAsync(int id) //Асинхронно перезагружает сущность из базы данных в отсоединенном состоянии.
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
        public TEntity ReloadForReload(int id) //Принудительно перезагружает сущность из базы данных с обновлением трекера и возвратом в отсоединенном состоянии.
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
        private void FillChangeLog(TEntity item, string operation) //Заполняет журнал изменений сущности данными о пользователе, операции и времени выполнения.
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
