using ORM.BaseClass;
using ORM.Context;

namespace ORM.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private DBContext<TEntity> Context;

        public Repository(DBContext<TEntity> context) 
        { 
            Context = context;
        }

        public void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity is null");
            }

           Context.DbSet.Insert(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            if(!entities.Any() || entities == null)
            {
                throw new ArgumentException("Entities are null");
            }

            Context.DbSet.Insert(entities);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.DbSet.Select();
        }

        public TEntity? GetById(int id)
        {
            if(id < 0)
            {
                return default;
            }
            
            return (TEntity)Context.DbSet.Select(x => x.Id == id);
        }

        public int Remove(IEnumerable<TEntity> entities)
        {
            return Context.DbSet.Delete(entities);
        }

        public int RemoveAll()
        {
            return Context.DbSet.DeleteAll();
        }

        public int RemoveById(int id)
        {
            TEntity? entity = GetById(id);
            if(entity != null)
            {
                return Context.DbSet.Delete(entity);
            }

            return 0;
            
        }

        public int Update(TEntity entity)
        {
           TEntity updatedEntity = GetById(entity.Id); 
            if(updatedEntity != null) 
            {
                return Context.DbSet.Update(updatedEntity);
            }

            return 0;
        }
    }
}
