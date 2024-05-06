using ORM.BaseClass;
using System.Linq.Expressions;

namespace ORM.Context
{
    public interface IDbSet<T> where T : IEntity
    {
        public IEnumerable<T> Select();

        public IEnumerable<T> Select(Expression<Func<T, bool>> expression);

        public void Insert(T entity);

        public void Insert(IEnumerable<T> entities);

        public void Update(T entity);

        public void Update(IEnumerable<T> entities);

        public int Delete(T entity);

        public int Delete(IEnumerable<T> entities);

        public int Delete(Expression<Func<T, bool>> expression);

        public int DeleteAll();
    }
}
