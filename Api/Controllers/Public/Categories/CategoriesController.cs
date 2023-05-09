
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using Api.Controllers.Public.Base;
using Api.Controllers.Public.Categories.Dto.Request;
using Api.Controllers.Public.Categories.Dto.Response;
using AutoMapper;
using Dal.Categories.Entity;
using Dal.Categories.Repositories.Interface;
using Dal.User.Entity;
using Logic.Managers.Categories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public.Categories;


public class CategoriesController : BasePublicController
{
    private readonly IMapper _mapper;
    private readonly ICategoriesManager _categoriesManager; 
    private readonly UserManager<UserDal> _userManager;

    public CategoriesController(ICategoriesManager categoriesManager, IMapper mapper, UserManager<UserDal> userManager)
    {
        _mapper = mapper;
        _categoriesManager = categoriesManager;
        _userManager = userManager;
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoriesModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.CreateCategories(token, newCategory);
        return Ok(new GetCategoryModelResponse(newCategory.Name, newCategory.Id, newCategory.Type));
    }
    
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryModelRequest model)
    {
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.UpdateAsync(newCategory);
        return Ok();
    }
    
    [HttpDelete("delete/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    { 
        _categoriesManager.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("allCategories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCategories()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        
        var listCategoriesIncome = await _categoriesManager.GetAllCategoriesByType(token, "income");
        var listCategoriesExpenses = await _categoriesManager.GetAllCategoriesByType(token, "expenses");
        var resultIncome = listCategoriesIncome
            .Select(x => _mapper.Map<GetCategoryModelResponse>(x))
            .ToList();
        var resultExpenses = listCategoriesExpenses
            .Select(x => _mapper.Map<GetCategoryModelResponse>(x))
            .ToList();
        return resultIncome != null && resultExpenses != null?
            Ok(new GetAllCategoryModelResponse(resultIncome, resultExpenses)) : BadRequest();
    }

    [HttpGet("getCategory/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id)
    {
        var category = await _categoriesManager.GetAsync(id);
        return category != null ? Ok(new GetCategoryModelResponse(category.Name, category.Id, category.Type)) : BadRequest();
    }
}