// Uweb4Media.Persistence.Repositories/Repository.cs
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Persistence.Context;

namespace Uweb4Media.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly Uweb4MediaContext _uweb4MediaContext;
        public Repository(Uweb4MediaContext uweb4MediaContext)
        {
            _uweb4MediaContext = uweb4MediaContext;
        }

        public async Task CreateAsync(T entity)
        {
            _uweb4MediaContext.Set<T>().Add(entity);
            await _uweb4MediaContext.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _uweb4MediaContext.Set<T>().ToListAsync();
        }

        // Düzeltilen GetByFilterAsync metodu
        public async Task<T?> GetByFilterAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _uweb4MediaContext.Set<T>();

            // 'includes' parametresindeki her bir navigasyon özelliğini sorguya dahil et
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }

            return await query.SingleOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _uweb4MediaContext.Set<T>().Where(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _uweb4MediaContext.Set<T>().FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            _uweb4MediaContext.Set<T>().Remove(entity);
            await _uweb4MediaContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _uweb4MediaContext.Set<T>().Update(entity);
            await _uweb4MediaContext.SaveChangesAsync();
        }
        // Repository.cs'e eklenecek method
        public async Task<List<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _weddingHallContext.Set<T>();

            // 'includes' parametresindeki her bir navigasyon özelliğini sorguya dahil et
            foreach (var includeProperty in includes)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }
    }
}