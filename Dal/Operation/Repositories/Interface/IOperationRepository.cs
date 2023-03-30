using Dal.Base.Repositories.Interface;
using Dal.Operation.Entity;

namespace Dal.Operation.Repositories.Interface;

public interface IOperationRepository : IBaseRepository<OperationDal, Guid>
{
    
}