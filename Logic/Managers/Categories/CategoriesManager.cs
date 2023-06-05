﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Dal.Base.Repositories.Interface;
using Dal.Categories.Entity;
using Dal.Categories.Repositories;
using Dal.Categories.Repositories.Interface;
using Dal.Operation.Entity;
using Dal.Operation.Repositories.Interface;
using Dal.User.Entity;
using Logic.Managers.Base;
using Logic.Managers.Categories.Interface;
using Microsoft.AspNetCore.Identity;

namespace Logic.Managers.Categories;

public class CategoriesManager : BaseManager<CategoriesDal, Guid>, ICategoriesManager
{
    private readonly UserManager<UserDal> _userManager;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IOperationRepository _operationRepository;
    public CategoriesManager(ICategoriesRepository repository, UserManager<UserDal> userManager, IOperationRepository operationRepository) : base(repository)
    {
        _userManager = userManager;
        _categoriesRepository = repository;
        _operationRepository = operationRepository;
    }

    private async Task<UserDal> FindUser(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        var user = await _userManager.FindByEmailAsync(email);
        return user;
    }
    
    public async Task AddStaticCategories(UserDal user)
    {
        var p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        var listStaticCategories = new List<CategoriesDal>()
        {
            new() { Id = Guid.NewGuid(), Name = "Продукты", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\1.png"},
            new() { Id = Guid.NewGuid(), Name = "Развлечение", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\2.png"},
            new() { Id = Guid.NewGuid(), Name = "Еда вне дома", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\3.png"},
            new() { Id = Guid.NewGuid(), Name = "Транспорт", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\4.png"},
            new() { Id = Guid.NewGuid(), Name = "Образование", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\5.png"},
            new() { Id = Guid.NewGuid(), Name = "Спорт", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\6.png"},
            new() { Id = Guid.NewGuid(), Name = "Подарки", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\7.png"},
            new() { Id = Guid.NewGuid(), Name = "Здоровье", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\8.png"},
            new() { Id = Guid.NewGuid(), Name = "Покупки", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\9.png"},
            new() { Id = Guid.NewGuid(), Name = "ЖКХ", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\10.png"},
            new() { Id = Guid.NewGuid(), Name = "Связь", Type = "expenses", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\11.png"},

            new() { Id = Guid.NewGuid(), Name = "Зарплата", Type = "income", UserDal = user, Img = $"{p}\\Dal\\wwwroot\\PictureForCategories\\Group(13).png" }
        };
        foreach (var category in listStaticCategories)          
        {
            await Repository.InsertAsync(category); 
        }
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

    public async Task DeleteCategory(Guid id)
    {
        var category = await GetAsync(id);
        var listOperation = await _categoriesRepository.GetOperations(id);
        foreach (var operation in listOperation)
        {
            await _operationRepository.DeleteAsync(operation.Id);
        }
        await DeleteAsync(id);
    }
}