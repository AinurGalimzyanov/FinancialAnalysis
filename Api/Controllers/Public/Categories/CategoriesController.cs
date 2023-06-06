
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("create")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoriesModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        if (CheckNotValidAccess(token)) return StatusCode(403);
        var newCategory = _mapper.Map<CategoriesDal>(model);
        var sum = await _categoriesManager.CreateCategories(token, newCategory);
        return Ok(new CategoryResponse(newCategory.Name, newCategory.Id, newCategory.Type, sum, newCategory.Img));
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryModelRequest model)
    {
        var newCategory = _mapper.Map<CategoriesDal>(model);
        await _categoriesManager.UpdateAsync(newCategory);
        return Ok();
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
    { 
        await _categoriesManager.DeleteCategory(id);
        return Ok();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            responsesIncome.Add(new CategoryResponse(dal.Name, dal.Id, dal.Type, await _categoriesManager.GetSumCategory(dal.Id, token), dal.Img));
        }
        foreach (var dal in categories.Item2)
        {
            responsesExpenses.Add(new CategoryResponse(dal.Name, dal.Id, dal.Type, await _categoriesManager.GetSumCategory(dal.Id, token), dal.Img));
        }
        return Ok(new AllCategoryByTypeResponse(responsesIncome, responsesExpenses));
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("getAllCategoryWithOperation")]
    public async Task<IActionResult> GetAllCategoryWithOperation([FromQuery] DateTimeLimitRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        if (CheckNotValidAccess(token)) return StatusCode(403);
        var categories = await _categoriesManager.GetAllAsync();
        var responses= new List<CategoryResponse>();
        foreach (var category in  categories)
        {
            responses.Add(new CategoryResponse(category.Name, category.Id, category.Type,
                await _categoriesManager.GetSumCategoryFromTo(token, category.Id, model.FromDateTime, model.ToDateTime,
                    model.Type)));
        }
        return Ok(new AllCategory(responses));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("getCategory/{id}")]
    public async Task<IActionResult> GetCategory([FromRoute] Guid id)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var sum = await _categoriesManager.GetSumCategory(id, token);
        var category = await _categoriesManager.GetAsync(id);
        return Ok(new CategoryResponse(category.Name, category.Id, category.Type, sum, category.Img));
    }
    
    private bool CheckNotValidAccess(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        return jwt.ValidTo < DateTime.UtcNow;
    }
    
    [HttpGet("getPictureForCategories/{img}")]
    public async Task<IActionResult> GetPictureForCategories([FromRoute] string img)
    {
        var p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        string path = $"{p}\\Dal\\wwwroot\\PictureForCategories\\" + img;
        var fileType="application/octet-stream";
        var fileStream = new FileStream(path, FileMode.Open);
        return Ok(fileStream);
    }
    
    [HttpGet("getUriPicturesForCategories")]
    public async Task<IActionResult> GetPicturesForCategories()
    {
        var p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        string path = $"{p}\\Dal\\wwwroot\\PictureForCategories";
        var responses = Directory
            .GetFiles(path)
            .Select(x => new PictureModelResponse(new Uri($"{Request.Scheme}://{Request.Host}/api/v1/public/Categories/getPictureForCategories/{x.Split("\\").LastOrDefault()}")))
            .ToList();
        return Ok(new PicturesModelResponse(responses));
    }
    
   
}