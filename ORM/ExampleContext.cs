using ORM.DataContextImplementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
