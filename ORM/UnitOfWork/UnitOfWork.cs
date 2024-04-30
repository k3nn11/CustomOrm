namespace ORM.UnitOfWorkImplementation
{
    internal class UnitOfWork<TContext> : IUnitOfWork<TContext>, IDisposable
    {
        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void CreateTransaction()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
