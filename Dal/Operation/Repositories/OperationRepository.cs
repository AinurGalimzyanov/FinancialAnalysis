using Dal.Operation.Entity;
using Dal.Operation.Repositories.Interface;

namespace Dal.Operation.Repositories;

public class OperationRepository : IOperationRepository
{
    public Task<Guid> InsertAsync(OperationDal dal)
    {
        throw new NotImplementedException();
    }

    public void DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<OperationDal?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> UpdateAsync(OperationDal dal)
    {
        throw new NotImplementedException();
    }
}

