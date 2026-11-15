using BeautyBook.Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BeautyBook.Data.Repositories.Implementations
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public IQueryable<TEntity> All()
            => dbSet;

        public IQueryable<TEntity> AllAsNoTracking()
            => dbSet.AsNoTracking();

        public async Task<TEntity?> GetByIdAsync(int id)
            => await dbSet.FindAsync(id);

        public async Task AddAsync(TEntity entity)
            => await dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => dbSet.Update(entity);

        public void Delete(TEntity entity)
            => dbSet.Remove(entity);

        public async Task<int> SaveChangesAsync()
            => await context.SaveChangesAsync();
    }
}