using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Controllers.Public.Auth.Dto.Request;
using Api.Controllers.Public.Auth.Dto.Response;
using Api.Controllers.Public.Base;
using AutoMapper;
using Dal.User.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog.Context;

namespace Api.Controllers.Public.Auth;

public class AuthorizeController : BasePublicController
{
    private readonly SignInManager<UserDal> _signInManager;
    private readonly UserManager<UserDal> _userManager;
    private readonly JWTSettings _options;
    private readonly IMapper _mapper;

    public AuthorizeController(UserManager<UserDal> userManager, 
        SignInManager<UserDal> signInManager, 
        IOptions<JWTSettings> options,
        IMapper mapper)
    {
        LogContext.PushProperty("Source", "Test Authorize Controller");
        _userManager = userManager;
        _signInManager = signInManager;
        _options = options.Value;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModelRequest model)
    {
        var user = _mapper.Map<UserDal>(model);
        var result = await _userManager.CreateAsync(user, model.Password);
        
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            await _userManager.AddClaimsAsync(user, claims);
            var accessToken = GetToken(claims, 15);
            var refreshToken = GetToken(claims, 43200);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", refreshToken);
            return Ok(new RegistModelResponse(accessToken, refreshToken, user.Name, user.Email));
        }

        return BadRequest();
    }
    
    private string GetToken(IEnumerable<Claim> principal, int timeMin)
    {
        var claims = principal.ToList();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.SecretKey));
        var token = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(timeMin),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInModelRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        
        var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, false, false);

        if (result.Succeeded)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
            return Ok(new SingInModelResponse(accessToken, refreshToken, user.Name, user.Email, user.Balance));
        }

        return Unauthorized();
    }

    /*
    private string СonvertStringInJwtAndReturnEmail(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return email;
    }
    */
    
    [HttpPost("signinWithAccess")]
    public async Task<IActionResult> SignInWithAccess([FromHeader] string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (email == null) return BadRequest();
        var user = await _userManager.FindByEmailAsync(email);

        if (user != null && jwt.ValidTo <= DateTime.Now) // нужно ли если jwt устарел обновить его сразу?
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
            return Ok(new SingInModelResponse(accessToken, refreshToken, user.Name, user.Email, user.Balance));
        }
        
        return Unauthorized();
    }
    
    [HttpPost("signout")]
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpPost("refreshAccessToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshModelRequest model)
    {
        if (model is null)
            return BadRequest("Invalid client request");
        
        string accessToken = model.AccessToken;
        string refreshToken = model.RefreshToken;
        
        var handler = new JwtSecurityTokenHandler();
        var jwtAccess = handler.ReadJwtToken(accessToken);
        var jwtRefresh = handler.ReadJwtToken(refreshToken);
        var email = jwtAccess.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        if (email == null) return BadRequest();
        var user = await _userManager.FindByEmailAsync(email);
        
        if(user != null && jwtRefresh.ValidTo <= DateTime.Now)
        {
            return Ok();
        }

        return BadRequest();
    }
}