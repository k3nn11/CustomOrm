using ORM.BaseClass;
using System;

namespace ORM.RepositoryImplementation
{
    internal interface IRepository<TEntity> where TEntity : IEntity
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(int id);

        int RemoveById(int id);

        int Remove(IEnumerable<TEntity> entities);

        int RemoveAll();

        int Update(TEntity entity);
    }
}
