using ORM.BaseClass;
using ORM.CustomAttribute;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace ORM
{
    public class Person : IEntity
    {
        [Column("Name", typeof(double))]
        public string Name { get; set; }

        [Column("Description", typeof(string), AllowNullable = true)]
        public string Description { get; set; }

        [Column("Rating", typeof (string), AllowNullable = true)]
        public int Rating { get; set; }
        public uint Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
