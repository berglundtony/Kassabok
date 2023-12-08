using Kassabok.Data;
using Microsoft.EntityFrameworkCore;

namespace Kassabok.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        private readonly TransactionsDbContext _transactionsDbContext;

        public Repository(TransactionsDbContext transactionsDbContext)
        {
            _dbSet = transactionsDbContext.Set<T>();
            _transactionsDbContext = transactionsDbContext;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> AddAccountAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _transactionsDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> GetByIdAsync(int? id)
        {
           return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Attach(entity);
            _transactionsDbContext.Entry(entity).State = EntityState.Modified;
            await _transactionsDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _transactionsDbContext.SaveChangesAsync();
        }

    }
}
