using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }
    
    public async Task CreateCategories(string token, CategoriesDal dal)
    {
        var user = await FindUser(token);
        dal.UserDal = user;
        Repository.InsertAsync(dal);
    }

    public async Task<List<CategoriesDal>> GetAll(string token)
    {
        var user = await FindUser(token);
        return _categoriesRepository.GetAllUserCategory(user.Id);
    }

    public async Task<int> GetExpenseAsync(string token, string nameCategory)
    {
        var user = await FindUser(token);
        var categories = user.CategoriesList.FirstOrDefault(x => x.Name == nameCategory);
        var expense = categories.OperationList.Select(x => x.Price).Sum();
        return 0;
    }

}