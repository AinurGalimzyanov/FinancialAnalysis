using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.Categories.Repositories;
using Dal.Categories.Repositories.Interface;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Categories.Interface;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.Categories;

public class CategoriesManager : BaseManager<CategoriesDal, Guid>, ICategoriesManager
{
    private readonly UserManager<UserDal> _userManager;
    private readonly ICategoriesRepository _categoriesRepository;
    public CategoriesManager(ICategoriesRepository repository, UserManager<UserDal> userManager) : base(repository)
    {
        _userManager = userManager;
        _categoriesRepository = repository;
    }

    public async Task CreateCategories(string userId, CategoriesDal dal)
    {
        var user = await _userManager.FindByIdAsync(userId);
        dal.UserDal = user;
        Repository.InsertAsync(dal);
    }

    public async Task<List<CategoriesDal>> GetAll(string userId)
    {
        return _categoriesRepository.GetAllUserCategory(userId);
    }

    public async Task<int> GetExpenseAsync(string userId, string nameCategory)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var categories = user.CategoriesList.FirstOrDefault(x => x.Name == nameCategory);
        var expense = categories.OperationList.Select(x => x.Price).Sum();
        return 0;
    }

}