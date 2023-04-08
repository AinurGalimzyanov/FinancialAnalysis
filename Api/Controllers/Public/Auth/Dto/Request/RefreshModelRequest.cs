using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Request;

public class RefreshModelRequest
{
    [Required] 
    [JsonProperty("AccessToken")] 
    public required string AccessToken { get; init; }
    
    [Required] 
    [JsonProperty("RefreshToken")] 
    public required string RefreshToken { get; init; }
}