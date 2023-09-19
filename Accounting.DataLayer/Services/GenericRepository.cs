using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataLayer.Services
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private Accounting_DBEntities _db;
        private DbSet<TEntity> _dbset;
        public GenericRepository(Accounting_DBEntities entity)
        {
            _db = entity;
            _dbset = _db.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> where = null)
        {
            IQueryable<TEntity> query = _dbset;
            if (where != null)
            {
                query = query.Where(where);
            }
            return query.ToList();
        }
        public TEntity GetById(object Id)
        {
            return _dbset.Find(Id);
        }
        public virtual bool Insert(TEntity entity)
        {
            try
            {
                _dbset.Add(entity);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public virtual bool Update(TEntity entity)
        {
            try
            {
                if (_db.Entry(entity).State == EntityState.Detached)
                {
                    _dbset.Attach(entity);
                }
                _db.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _dbset.Attach(entity);
            _db.Entry(entity).State = EntityState.Deleted;
        }
        public virtual void Delete(object Id)
        {
            var customer = GetById(Id);
            Delete(customer);
        }
    }
}
