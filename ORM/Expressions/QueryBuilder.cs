using System.Linq.Expressions;
using System.Text;

namespace ORM.Expressions
{
    public class QueryBuilder : ExpressionVisitor
    {

        private StringBuilder? _sb;

        public string? WhereClause { get; private set; }

        public string Translate(Expression expression)
        {
           _sb = new StringBuilder();
            this.Visit(expression);
            WhereClause = _sb.ToString();
            return WhereClause;
        }
    }
}
