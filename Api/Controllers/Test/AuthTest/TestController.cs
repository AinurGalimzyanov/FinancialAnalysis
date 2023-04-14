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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Test([FromBody] SignInModelRequest modelRequest)
    {
        return Ok();
    }
}