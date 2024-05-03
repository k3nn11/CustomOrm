using Microsoft.Identity.Client;
using ORM.BaseClass;
using System;

namespace ORM.Manager
{
    public interface ITableIO<T> where T : IEntity
    {
        public IEnumerable<T> Select();

        public void Insert(T entity);

        public void Insert(IEnumerable<T> entities);

        public void Update(T entity);

        public void Update(IEnumerable<T> entities);

        public int Delete(T entity);

        public int Delete(IEnumerable<T> entities);

        public int DeleteAll();
    }
}
