using Dal.Categories.Entity;
using Logic.Managers.Base.Interface;

namespace Logic.Managers.Categories.Interface;

public interface ICategoriesManager : IBaseManager<CategoriesDal, Guid>
{
     public Task<int?> CreateCategories(string token, CategoriesDal dal);
     
     public Task<(List<CategoriesDal>,List<CategoriesDal>)> GetAllCategoriesByType(string token);

     public Task<int?> GetSumCategory(Guid categoryId);
}