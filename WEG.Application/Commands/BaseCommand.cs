using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEG.Domain.Entities;

namespace WEG.Application.Commands
{
    public class BaseCommand<TEntity, TIdType> : IBaseCommand<TEntity, TIdType> where TEntity : class, IEntity<TIdType>
    {
        protected readonly ApplicationDbContext context;
        public BaseCommand(ApplicationDbContext context)
        {
            this.context = context;
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await context.AddAsync(entity);

            return entity;
        }
        public virtual void Delete(TIdType id)
        {
            var entity = context.Set<TEntity>().Find(id);

            if (entity == null)
            {
                throw new Exception("There is no entity with this id.");
            }

            context.Remove(entity);
        }
        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public virtual void Update(TEntity entity)
        {
            var temp = context.Find<TEntity>(entity.Id);

            if (temp == null)
            {
                throw new Exception("There is no entity with this id.");
            }

            context.Entry(temp).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            context.Update(entity);
        }
    }
}
