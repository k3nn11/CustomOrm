namespace ORM.UnitOfWorkImplementation
{
    internal class UnitOfWork<TContext> : IUnitOfWork<TContext>
    {
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
