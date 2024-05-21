
namespace ORM.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AutoIncrement : Attribute
    {
        public int AutoIncrementSeed {  get; set; }
        public int AutoIncrementStep { get; set; }
        public AutoIncrement() 
        {
            AutoIncrementSeed = 1;
            AutoIncrementStep = 1;
        }
    }
}
