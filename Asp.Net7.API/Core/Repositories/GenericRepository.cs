using Asp.Net7.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Asp.Net7.API.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApiDbContext _context;
        internal DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        public GenericRepository(ApiDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            this._dbSet = _context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T?> GetById(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Entry(entity).State = EntityState.Detached; // Aynı oturumda Aynı class'ın nesnesine erişip üzerinde işlem yapıp ardından Update yaparsak hata alırız. Bu yüzden entity nesnesi üzerindeki değişikliklerin takip edilmemesini bu şekilde sağlayabilirsiniz.
            return entity;
        }

        public virtual async Task<bool> Update(T entity)
        {
            _dbSet.Update(entity);
            return true;
        }


    }
}
