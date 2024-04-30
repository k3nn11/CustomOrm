using ORM.CustomAttribute;

namespace ORM.BaseClass
{
    public interface IEntity
    {
        [Primary]
        uint Id { get; set; }
    }
}
