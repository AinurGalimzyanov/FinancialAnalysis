using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Response;

public class RegistModelResponse
{
    [Required] 
    [JsonProperty("AccessToken")] 
    public string AccessToken { get; init; }
    
    [Required] 
    [JsonProperty("RefreshToken")] 
    public string RefreshToken { get; init; }

    [Required] 
    [JsonProperty("Name")] 
    public string Name { get; init; }
    
    [Required] 
    [EmailAddress]
    [JsonProperty("Email")] 
    public string Email { get; init; }

    public RegistModelResponse(string accessToken, string refreshToken, string name, string email)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Name = name;
        Email = email;
    }
}