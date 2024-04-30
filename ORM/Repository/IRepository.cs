using System;

namespace ORM.RepositoryImplementation
{
    internal interface IRepository<TClass> where TClass : class
    {
        IEnumerable<TClass> GetAll();

        TClass FindById(int id);

        void Add(TClass entity);

        void RemoveById(int id);

        void Update(TClass entity);
    }
}
