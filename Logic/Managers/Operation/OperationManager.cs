using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Dal.Operation.Entity;
using Dal.Operation.Repositories.Interface;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Categories.Interface;
using Logic.Managers.Operation.Interface;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.Operation;

public class OperationManager : BaseManager<OperationDal, Guid>, IOperationManager
{
    private readonly UserManager<UserDal> _userManager;
    private readonly IOperationRepository _operationRepository;
    private readonly ICategoriesRepository _categoriesRepository;

    public OperationManager(IOperationRepository repository,
        UserManager<UserDal> userManager, ICategoriesRepository categoriesRepository) : base(repository)
    {
        _userManager = userManager;
        _operationRepository = repository;
        _categoriesRepository = categoriesRepository;
    }

    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return null;
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return await _userManager.FindByEmailAsync(email);
        ;
    }

    private DateTime GetDateWithUtc(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    public async Task CreateOperation(string token, OperationDal dal, Guid categoryId)
    {
        var user = await FindUser(token);
        dal.CategoriesDal = await _categoriesRepository.GetAsync(categoryId);
        dal.UserDal = user;
        dal.DateTime = GetDateWithUtc(dal.DateTime.Value);
        var currentPrice = dal.CategoriesDal.Type == "income" ? dal.Price : -dal.Price;
        var isSet = await SetBalanceAfterNewOperation(user, currentPrice);
        if (isSet)
        {
            await _operationRepository.InsertAsync(dal);
        }
    }

    private async Task<int?> GetSumByType(string userId, string type)
    {
        var categoryTypeList = await _operationRepository.GetAllOperationByTypeAsync(userId, type, DateTime.UtcNow);
        return categoryTypeList.Count != 0 ? categoryTypeList.Select(x => x.Price).Sum() : 0;
    }

    private async Task<bool> SetBalanceAfterNewOperation(UserDal user, int? currentPrice)
    {
        var balance = await GetSumByType(user.Id, "income") - await GetSumByType(user.Id, "expenses") + currentPrice;

        if (balance >= 0)
        {
            user.Balance = balance;
            await _userManager.UpdateAsync(user);
            return true;
        }

        return false;
    }

    public async Task<List<OperationDal>> GetOperationsByTypeAsync(string token, string type, DateTime date)
    {
        var user = await FindUser(token);
        return await _operationRepository.GetAllOperationByTypeAsync(user.Id, type, date);
    }



    public async Task<List<OperationDal>> GetOperationsByCategoryAsync(string token, Guid categoryId, DateTime date)
    {
        var user = await FindUser(token);
        return await _operationRepository.GetAllOperationByCategoryAsync(user.Id, categoryId, date);
    }

    public async Task<int?> GetBalanceAsync(string token)
    {
        var user = await FindUser(token);
        return user.Balance;
        ;
    }


    public async Task CreateBalanceAsync(string token, int newBalance)
    {
        var user = await FindUser(token);
        user.Balance = newBalance;
        await _userManager.UpdateAsync(user);
    }

    public async Task<List<OperationDal>> GetLastFiveOperationsAsync(string token, DateTime date)
    {
        var user = await FindUser(token);
        var allOperations = await _operationRepository.GetAllOperationsAsync(user.Id, date);
        return allOperations.Take(5).ToList();
    }

    public async Task<List<OperationDal>> GetLastFiveOperationsBothTypeAsync(string token, string type, DateTime date)
    {
        var user = await FindUser(token);
        var allOperations = await _operationRepository.GetAllOperationByTypeAsync(user.Id, type, date);
        return allOperations.Take(5).ToList();
    }
}