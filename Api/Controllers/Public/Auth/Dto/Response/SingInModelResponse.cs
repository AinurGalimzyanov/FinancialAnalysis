using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Response;

public class SingInModelResponse
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
    
    [Required]
    [JsonProperty("Balance")]
    public int? Balance { get; init; }

    public SingInModelResponse(string accessToken, string refreshToken, string name, string email, int? balance)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Name = name;
        Email = email;
        Balance = balance;
    }
}