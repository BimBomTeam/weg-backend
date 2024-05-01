using WEG.Domain.Entities;
using WEG.Infrastructure.Queries;

namespace WEG.Application.Queries
{
    public class BaseQuery<TEntity, TIdType> : IBaseQuery <TEntity, TIdType> where TEntity : class, IEntity<TIdType>
    {
        protected readonly ApplicationDbContext context;

        public BaseQuery(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>().AsQueryable<TEntity>();
        }
        public async Task<TEntity> GetByIdAsync(TIdType id)
        {
            var entity = await context.Set<TEntity>().FindAsync(id);

            if (entity == null)
            {
                throw new Exception("There is no entity with this id.");
            }

            return entity;
        }
    }
}
