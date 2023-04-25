using Dal.Categories.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Categories.Interface;

public interface ICategoriesManager : IBaseManager<CategoriesDal, Guid>
{
     public Task CreateCategories(string userId, CategoriesDal dal);
     
     public Task<List<CategoriesDal>> GetAll(string userId);

     public Task<int> GetExpenseAsync(string userId, string nameCategory);
}