using Dal.Categories.Entity;
using Dal.Operation.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Operation.Interface;

public interface IOperationManager : IBaseManager<OperationDal, Guid>
{
    public Task CreateOperation(string token, OperationDal dal, Guid categoryId);
    public Task<List<OperationDal>> GetOperationsByType(string token, Guid categoryId, string type);
    
    
}