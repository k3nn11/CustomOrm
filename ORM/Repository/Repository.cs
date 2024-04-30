using System;

namespace ORM.RepositoryImplementation
{
    internal class Repository<TClass> : IRepository<TClass> where TClass : class
    {
        public void Add(TClass entity)
        {
            throw new NotImplementedException();
        }

        public TClass FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TClass> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(TClass entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TClass entity)
        {
            throw new NotImplementedException();
        }
    }
}
