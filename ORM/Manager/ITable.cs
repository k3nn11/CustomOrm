namespace ORM.Manager
{
    public interface ITable
    {
        string Name { get; }

        void CreateTable();

        void UpdateTable();

    }
}
