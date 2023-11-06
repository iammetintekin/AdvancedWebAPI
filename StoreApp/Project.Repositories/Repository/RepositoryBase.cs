using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Repositories.Repository
{
    // we are blocking not to creating new instance of it. 
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private DbSet<T> _dbSet;
        // this is accessible for the classes inherited from RepositoryBase
        // protected readonly DbContext _databaseContext;
        public RepositoryBase(DbContext databaseContext)
        {
            //_databaseContext = databaseContext;
            _dbSet = databaseContext.Set<T>();
        }
        public async Task CreateAsync(T entity) => _dbSet.Add(entity);
        public async Task DeleteAsync(T entity) => _dbSet.Remove(entity);
        public async Task UpdateAsync(T entity) => _dbSet.Update(entity);
        //AsNoTracking fonksiyonu ile takibi kırılmış tüm nesneler doğal olarak güncelleme durumlarında
        //“SaveChanges” fonksiyonundan etkilenmeyecektirler.
        //Kimi durumda vt deki nesneyi güncellemek isteyebiliriz, kimi durumda istemeyebiliriz.
        // bu yüzden bool değişkene atadık bu durumu, performansı etkiler.
        public async Task<IQueryable<T>> FindAllAsync(bool TrackChanges)
            => TrackChanges ?
            _dbSet.AsNoTracking() :
            _dbSet;

        public async Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool TrackChanges)
             => TrackChanges ?
                _dbSet.Where(expression).AsNoTracking() :
                _dbSet.Where(expression);

    }
}
