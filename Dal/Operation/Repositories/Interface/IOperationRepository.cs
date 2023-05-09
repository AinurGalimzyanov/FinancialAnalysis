using Dal.Base.Repositories.Interface;
using Dal.Operation.Entity;

namespace Dal.Operation.Repositories.Interface;

public interface IOperationRepository : IBaseRepository<OperationDal, Guid>
{
    public Task<List<OperationDal>> GetAllOperationByCategoryAsync(string userId, Guid categoryId, DateTime date);
    public Task<List<OperationDal>> GetAllOperationByTypeAsync(string userId, string type, DateTime date);
    public Task<List<OperationDal>> GetAllOperationsAsync(string userId, DateTime date);
    
    
}