using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Tickets.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly TicketContext Context;
        private DbSet<TEntity> _entities;
        public Repository(TicketContext context)
        {
            Context = context;
            _entities = Context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _entities.Add(entity);
            //throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entities.AddRange(entities);
            //throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate);
            //throw new NotImplementedException();
        }

        public virtual TEntity Get(int id)
        {
            return _entities.Find(id);
            //throw new NotImplementedException();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _entities.ToList();
            //throw new NotImplementedException();
        }

        public void Remove(TEntity entity)
        {
            _entities.Remove(entity);
            //throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entities.RemoveRange(entities);
            //throw new NotImplementedException();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
            //throw new NotImplementedException();
        }
    }
}
