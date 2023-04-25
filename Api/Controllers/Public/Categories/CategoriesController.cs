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

    public CategoriesController(ICategoriesManager categoriesManager, IMapper mapper)
    {
        _mapper = mapper;
        _categoriesManager = categoriesManager;
    }

    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> CreateCategory([FromHeader] string userId, [FromBody] CreateCategoriesModelRequest model)
    {
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.CreateCategories(userId, newCategory);
        return Ok(newCategory.Id);
    }
    
    [HttpPost("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryModelRequest model)
    {
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.UpdateAsync(newCategory);
        return Ok();
    }
    
    [HttpPost("delete/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    { 
        _categoriesManager.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("allCategories")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllCategories([FromHeader] string userId)
    {
        var listCategoriesDals = await _categoriesManager.GetAll(userId);
        var result = listCategoriesDals
            .Select(x => _mapper.Map<GetCategoryModelResponse>(x))
            .ToList();
        return result != null ? Ok(new GetAllCategoryModelResponse(result)) : BadRequest();
    }
    
    [HttpGet("expenseCategory/{name}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetExpenseCategory([FromHeader] string userId, [FromRoute] string name)
    {
        var result = await _categoriesManager.GetExpenseAsync(userId, name);
        return result != null ? Ok(result) : BadRequest();
    }
    
    [HttpGet("getCategory/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id)
    {
        var category = await _categoriesManager.GetAsync(id);
        return category != null ? Ok(new GetCategoryModelResponse(category.Name, category.Id)) : BadRequest();
    }
}