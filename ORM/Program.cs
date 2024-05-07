using ORM.ConnectionFactory;
using ORM.Context;
using ORM.Repository;
using ORM.Schema;
using System.Reflection;

namespace ORM
{
    public class Program
    {
        static void Main(string[] args)
        {
            User person = new User();
            person.Id = 1;
            person.Description = "Description";
            person.Rating = 1;
            person.Name = "James";

            string connection = "Data Source=(localdb)\\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            Assembly assembly = Assembly.GetExecutingAssembly();
            IProvider provider = new SqlProvider(connection);
            DataBaseManager dbmanager = new("CustomOrm", provider, assembly);
            IDbSet<User> set = (IDbSet<User>)dbmanager.GetTable(typeof(User));
            DBContext<User> context = new(set);
            Repository<User> repository = new(context);
            repository.Add(person);
        }
    }
}