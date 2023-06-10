using Api.Controllers.Public.Auth.Dto.Request;
using Api.Controllers.Public.Base;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Public.Auth;



public class TestController : BasePublicController
{
    private readonly IMapper _mapper;
    public TestController(IMapper mapper)
    {
        _mapper = mapper;
    }

    
    [HttpPost("test")]
    public async Task<IActionResult> Test()
    {
        var p = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        return Ok(p);
    }
    
    [HttpPost("test1")]
    public async Task<IActionResult> Test1()
    {
        string path = Directory.GetCurrentDirectory();
        
        return Ok(path);
    }
    
    [HttpPost("test2")]
    public async Task<IActionResult> Test2()
    {
        var p = "Dal/";
        string[] dirs = Directory.GetDirectories(p);
        
        return Ok(dirs);
    }
    
    [HttpPost("test3")]
    public async Task<IActionResult> Test3()
    {
        var p = "Dal";
        var fullPath = Path.GetFullPath(p);
        
        return Ok(fullPath);
    }
    
}