using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.Categories.Repositories;
using Dal.Categories.Repositories.Interface;
using Dal.Operation.Entity;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Categories.Interface;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.Categories;

public class CategoriesManager : BaseManager<CategoriesDal, Guid>, ICategoriesManager
{
    private readonly UserManager<UserDal> _userManager;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IMapper _mapper;
    public CategoriesManager(ICategoriesRepository repository, UserManager<UserDal> userManager, IMapper mapper) : base(repository)
    {
        _userManager = userManager;
        _categoriesRepository = repository;
        _mapper = mapper;
    }

    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }
    
    public async Task<int?> CreateCategories(string token, CategoriesDal dal)
    {
        var user = await FindUser(token);
        var sum = await _categoriesRepository.GetSumCategory(dal.Id, user.Id);
        dal.UserDal = user;
        await Repository.InsertAsync(dal);
        return sum;
    }

    public async Task<(List<CategoriesDal>, List<CategoriesDal>)> GetAllCategoriesByType(string token)
    {
        var user = await FindUser(token);
        var listIncome = await _categoriesRepository.GetAllUserCategory(user.Id, "income");
        var listExpenses = await _categoriesRepository.GetAllUserCategory(user.Id, "expenses");
        
        
        return new(listIncome, listExpenses);
    }

    public async Task<int?> GetSumCategory(Guid categoryId, string token)
    {
        var user = await FindUser(token);
        var category = await GetAsync(categoryId);
        var sum = await _categoriesRepository.GetSumCategory(categoryId, user.Id);
        return sum;
    }

    public async Task<List<Tuple<CategoriesDal, List<OperationDal>>>> GetCategoryWithOperations(string token, DateTime from, DateTime to)
    {
        var user = await FindUser(token);
        var list = await _categoriesRepository.GetCategoryWithOperation(user.Id, from, to);
        return list;
    }
}