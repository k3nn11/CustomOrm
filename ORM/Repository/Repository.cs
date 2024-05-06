using ORM.BaseClass;
using ORM.Context;

namespace ORM.RepositoryImplementation
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
            Context.DbSet.Select();
        }

        public TEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Remove(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public int RemoveAll()
        {
            throw new NotImplementedException();
        }

        public int RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        public int Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
