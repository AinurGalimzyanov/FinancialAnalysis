using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Controllers.Public.Auth.Dto.Request;
using Api.Controllers.Public.Auth.Dto.Response;
using Api.Controllers.Public.Base;
using AutoMapper;
using Dal.User.Entity;
using Logic.Managers.User.Interface;
using Microsoft.AspNetCore.Authorization;
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
            var claims = new List<Claim>()
            {
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Name)
            };
            await _userManager.AddClaimsAsync(user, claims);
            var accessToken = GetToken(claims, 15);
            var refreshToken = GetToken(claims, 43200);
            user.RefreshToken = refreshToken;
            await _userManager.UpdateAsync(user);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", refreshToken);
            var link = "http://localhost:5216/api/v1/public/Authorize/signin/{user.Id}";
            //EmailSender.SendEmail($"<a href=\"{link}\">{link}</a>", "vladimir.tereshin@urfu.me");
            return Ok(new RegistModelResponse("Bearer " + accessToken, user.Name, user.Email));
        }

        return BadRequest();
    }

    private async Task<UserDal> FindUserByToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);
        if (jwt.ValidTo < DateTime.UtcNow) return null;
        var email = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email)?.Value;
        return await _userManager.FindByEmailAsync(email);;
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
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(timeMin),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return tokenHandler.WriteToken(token);
    }
    
    /*[HttpPost("signin/{id}")]
    public async Task<IActionResult> SignIn([FromRoute] string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (!user.CheckExistenceMail)
        {
            user.CheckExistenceMail = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);
        
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
            if (refreshToken == null)
            { 
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", user.RefreshToken);
            }
            return Ok(new SingInModelResponse("Bearer " + accessToken, user.Name, user.Email, user.Balance));
        }
        return BadRequest("Вы уже подтвердили почту");
    }*/

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInModelRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, false, false);
        
        if (result.Succeeded ) //&& user.CheckExistenceMail
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
            if (refreshToken == null)
            { 
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", user.RefreshToken);
            }
            return Ok(new SingInModelResponse("Bearer " + accessToken, user.Name, user.Email, user.Balance));
        }

        return Unauthorized();
    }

    [HttpPost("signinWithAccess")]
    public async Task<IActionResult> SignInWithAccess()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var user = await FindUserByToken(token);
        
        if (user != null ) //&& user.CheckExistenceMail
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var accessToken = GetToken(claims, 15);
            return Ok(new SingInModelResponse("Bearer " +accessToken, user.Name, user.Email, user.Balance));
        }
        
        return Unauthorized();
    }
    
    [HttpPost("signout")]
    public async Task<IActionResult> SignOut()
    {
        await _signInManager.SignOutAsync();
        HttpContext.Response.Cookies.Delete(".AspNetCore.Application.RefreshToken");
        HttpContext.Response.Headers.Remove("Authorization");
        return Ok();
    }
    
    [HttpPatch("recoverPassword")]
    public async Task<IActionResult> RecoverPassword([FromQuery] RecoverPasswordModelRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var newPassword = Guid.NewGuid().ToString();
        await _userManager.RemovePasswordAsync(user);
        await _userManager.AddPasswordAsync(user, newPassword);
        EmailSender.SendEmail($"Новый пароль : {newPassword}", "aricsybsn@gmail.com");
        return Ok();
    }

    [HttpPatch("refreshAccessToken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = HttpContext.Request.Cookies[".AspNetCore.Application.RefreshToken"];
        var user = await FindUserByToken(refreshToken);
        
        if(user != null || user.RefreshToken == refreshToken)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var newAccessToken = GetToken(claims, 15);
            var newRefreshToken = GetToken(claims, 43200);
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", newRefreshToken);
            return Ok(new RefreshModelResponse("Bearer " + newAccessToken));
        }

        
        return BadRequest();
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUser()
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var user = await FindUserByToken(token);
        await _userManager.DeleteAsync(user);
        return Ok();
    }

    [HttpPut("updateUser")]
    public async Task<IActionResult> UpdateUser([FromQuery] UpdateUserModelRequest model)
    {
        var token = HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
        var user = await FindUserByToken(token);
        if (user != null)
        {
            user.Name = model.Name != null ? model.Name : user.Name;
            user.Email = model.Email != null ? model.Email : user.Email;
            user.UserName = model.Email != null ? model.Email : user.Email;
            if (model.Password != null)
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, model.Password);
            }
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                var claims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, claims);
                
                var newClaims = new List<Claim>()
                {
                    new(ClaimTypes.Name, user.Name),
                    new(ClaimTypes.Email, user.Email)
                };
                await _userManager.AddClaimsAsync(user, newClaims);
                
                var newAccessToken = GetToken(newClaims, 15);
                var newRefreshToken = GetToken(newClaims, 43200);
                HttpContext.Response.Cookies.Append(".AspNetCore.Application.RefreshToken", newRefreshToken);
                user.RefreshToken = newRefreshToken;
                await _userManager.UpdateAsync(user);
                
                return Ok(new UpdateUserModelResponse("Bearer " + newAccessToken));
            }
        }

        return BadRequest();
    }

}