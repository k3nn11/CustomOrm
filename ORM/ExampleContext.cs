using ORM.DataContextImplementation;

namespace ORM
{
    internal class ExampleContext : DataContext
    {
        public ExampleContext(string connection) 
            :base(connection)
        {
        }
        public TableContext<Set> Sets => GetTable<Set>();
    }
}
