using ORM.BaseClass;
using ORM.Context;
using System.Linq.Expressions;

namespace ORM.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private IDbSet<TEntity> _dbSet;

        public Repository(DBContext context) 
        { 
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is null");
            }

           _dbSet.Insert(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if(!entities.Any() || entities == null)
            {
                throw new ArgumentException("Entities are null");
            }

            _dbSet.Insert(entities);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.Select();
        }

        public TEntity? GetById(int id)
        {
            if(id < 0)
            {
                return default;
            }

            Expression<Func<TEntity, bool>> expression = x => x.Id == 2;
            IEnumerable<TEntity> items = _dbSet.Select(expression);
            return items.FirstOrDefault();
        }

        public int Remove(IEnumerable<TEntity> entities)
        {
            return _dbSet.Delete(entities);
        }

        public int RemoveAll()
        {
            return _dbSet.DeleteAll();
        }

        public int RemoveById(int id)
        {
            TEntity? entity = GetById(id);
            if(entity != null)
            {
                return _dbSet.Delete(entity);
            }

            return 0;
            
        }

        public int Update(TEntity entity)
        {
           TEntity updatedEntity = GetById(entity.Id); 
            if(updatedEntity != null) 
            {
                return _dbSet.Update(updatedEntity);
            }

            return 0;
        }
    }
}
