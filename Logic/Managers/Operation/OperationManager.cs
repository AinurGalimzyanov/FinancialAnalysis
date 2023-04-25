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

    public async Task CreateOperation(string userId, OperationDal dal, Guid categoryId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var category = await _categoriesRepository.GetAsync(categoryId);
        dal.CategoriesDal = category;
        dal.UserDal = user;
        var expensesList = await GetOperationByType(userId, categoryId, "expanses");
        var incomeList = await GetOperationByType(userId, categoryId, "income");
        var expenses = expensesList.Count != 0 ? expensesList.Select(x => x.Price).Sum() : 0;
        var income = incomeList.Count != 0 ? incomeList.Select(x => x.Price).Sum() : 0;
        if (income - expenses >= 0)
        {
            user.Balance = income - expenses;
            await _userManager.UpdateAsync(user);
            await _operationRepository.InsertAsync(dal);
        }
    }

    public async Task<List<OperationDal>> GetAllOperation(string userId, Guid categoryId)
    {
        return _operationRepository.GetAllUserCategoryOperation(userId, categoryId);
    }

    public async Task<List<OperationDal>> GetOperationByType(string userId, Guid categoryId, string typeOperation)
    {
        var operations = _operationRepository.GetAllUserCategoryOperation(userId, categoryId);
        return operations.Where(x => x.Type == typeOperation).ToList();
    }
}