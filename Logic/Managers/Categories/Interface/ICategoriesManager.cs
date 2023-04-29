using Dal.Categories.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Categories.Interface;

public interface ICategoriesManager : IBaseManager<CategoriesDal, Guid>
{
     public Task CreateCategories(string token, CategoriesDal dal);
     
     public Task<List<CategoriesDal>> GetAllCategoriesByType(string token, string type);

     public Task<int> GetExpenseAsync(string token, string nameCategory);
}