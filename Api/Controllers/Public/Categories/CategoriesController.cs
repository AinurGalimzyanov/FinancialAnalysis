
using System.IdentityModel.Tokens.Jwt;
using Api.Controllers.Public.Base;
using Api.Controllers.Public.Categories.Dto.Request;
using Api.Controllers.Public.Categories.Dto.Response;
using AutoMapper;
using Dal.Categories.Entity;
using Dal.User.Entity;
using Logic.Managers.Categories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public.Categories;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoriesModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        if (CheckNotValidAccess(token)) return StatusCode(403);
        var newCategory = _mapper.Map<CategoriesDal>(model);
        var sum = await _categoriesManager.CreateCategories(token, newCategory);
        return Ok(new CategoryResponse(newCategory.Name, newCategory.Id, newCategory.Type, sum));
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryModelRequest model)
    {
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.UpdateAsync(newCategory);
        return Ok();
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    { 
        await _categoriesManager.DeleteAsync(id);
        return Ok();
    }

    [HttpGet("allCategories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        if (CheckNotValidAccess(token)) return StatusCode(403);
        var categories = await _categoriesManager.GetAllCategoriesByType(token);
        var responsesIncome = new List<CategoryResponse>();
        var responsesExpenses = new List<CategoryResponse>();
        foreach (var dal in categories.Item1)
        {
            responsesIncome.Add(new CategoryResponse(dal.Name, dal.Id, dal.Type, await _categoriesManager.GetSumCategory(dal.Id)));
        }
        foreach (var dal in categories.Item2)
        {
            responsesExpenses.Add(new CategoryResponse(dal.Name, dal.Id, dal.Type, await _categoriesManager.GetSumCategory(dal.Id)));
        }
        return Ok(new AllCategoryByTypeResponse(responsesIncome, responsesExpenses));
    }

    [HttpGet("getCategory/{id}")]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id)
    {
        var sum = await _categoriesManager.GetSumCategory(id);
        var category = await _categoriesManager.GetAsync(id);
        return Ok(new CategoryResponse(category.Name, category.Id, category.Type, sum));
    }
    
    private bool CheckNotValidAccess(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo < DateTime.UtcNow;
    }
}