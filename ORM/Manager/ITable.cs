using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Manager
{
    public interface ITable
    {
        string Name { get; }

        void CreateTable();

    }
}
