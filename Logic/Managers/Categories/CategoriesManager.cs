using Dal.Categories.Entity;
using Dal.Categories.Repositories;
using Logic.Managers.Categories.Interface;

namespace Logic.Managers.Categories;

public class CategoriesManager : ICategoriesManager
{
    private CategoriesRepository _categoriesRepository;

    public CategoriesManager(CategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    /*public async Task<CategoriesDal> GetCategories(string name)
    {
        
    }*/
}