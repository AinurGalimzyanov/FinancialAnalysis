using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.Operation.Entity;
using Dal.User.Entity;

namespace Dal.Categories.Repositories.Interface;

public interface ICategoriesRepository : IBaseRepository<CategoriesDal, Guid>
{
    public Task<List<CategoriesDal>> GetAllUserCategory(string userId, string type);
    public Task<int?> GetSumCategory(Guid catId, string userId);

    public Task<List<Tuple<CategoriesDal, List<OperationDal>>>> GetCategoryWithOperation(string userId, DateTime from, DateTime to, string type);
    public Task<List<OperationDal>> GetOperations(Guid id);

}