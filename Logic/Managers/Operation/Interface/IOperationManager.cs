using Dal.Categories.Entity;
using Dal.Operation.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Operation.Interface;

public interface IOperationManager : IBaseManager<OperationDal, Guid>
{
    public Task CreateOperation(string token, OperationDal dal, Guid categoryId);
    public Task<List<OperationDal>> GetOperationsByTypeAsync(string token, string type, DateTime date);
    public Task<List<OperationDal>> GetOperationsByCategoryAsync(string token, Guid categoryId, DateTime date);
    public Task<int?> GetBalanceAsync(string token);
    public Task CreateBalanceAsync(string token, int newBalance);
    public Task<List<OperationDal>> GetLastFiveOperationsAsync(string token, DateTime date);
    public Task<List<OperationDal>> GetLastFiveOperationsBothTypeAsync(string token, string type, DateTime date);


}