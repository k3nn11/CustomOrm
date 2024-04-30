namespace ORM.UnitOfWorkImplementation
{
    internal interface IUnitOfWork<TContext> 
    {
        void CreateTransaction();
        void Commit();
        void Rollback();

    }
}
