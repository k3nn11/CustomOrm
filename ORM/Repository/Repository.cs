using ORM.BaseClass;
using ORM.Context;

namespace ORM.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private readonly DBContext<TEntity> _context;

        public Repository(DBContext<TEntity> context) 
        { 
            _context = context;
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is null");
            }

           _context.DbSet.Insert(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if(!entities.Any() || entities == null)
            {
                throw new ArgumentException("Entities are null");
            }

            _context.DbSet.Insert(entities);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.DbSet.Select();
        }

        public TEntity? GetById(int id)
        {
            if(id < 0)
            {
                return default;
            }
            
            return (TEntity)_context.DbSet.Select(x => x.Id == id);
        }

        public int Remove(IEnumerable<TEntity> entities)
        {
            return _context.DbSet.Delete(entities);
        }

        public int RemoveAll()
        {
            return _context.DbSet.DeleteAll();
        }

        public int RemoveById(int id)
        {
            TEntity? entity = GetById(id);
            if(entity != null)
            {
                return _context.DbSet.Delete(entity);
            }

            return 0;
            
        }

        public int Update(TEntity entity)
        {
           TEntity updatedEntity = GetById(entity.Id); 
            if(updatedEntity != null) 
            {
                return _context.DbSet.Update(updatedEntity);
            }

            return 0;
        }
    }
}
