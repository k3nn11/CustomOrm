using ORM.BaseClass;
using System.Linq.Expressions;

namespace ORM.Context
{
    public interface IDbSet<T>
    {
        public IEnumerable<T> Select();

        public IEnumerable<T> Select(Expression<Func<T, bool>> expression);

        public void Insert(T entity);

        public void Insert(IEnumerable<T> entities);

        public int Update(T entity);

        public int Update(IEnumerable<T> entities);

        public int Delete(T entity);

        public int Delete(IEnumerable<T> entities);

        public int Delete(Expression<Func<T, bool>> expression);

        public int DeleteAll();
    }
}
