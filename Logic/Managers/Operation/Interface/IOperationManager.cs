using Dal.Categories.Entity;
using Dal.Operation.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Operation.Interface;

public interface IOperationManager : IBaseManager<OperationDal, Guid>
{
    public Task CreateOperation(string userId, OperationDal dal, Guid categoryId);
    public Task<List<OperationDal>> GetAllOperation(string userId, Guid categoryId);
    public Task<List<OperationDal>> GetOperationByType(string userId, Guid categoryId, string typeOperation);
    
    
}