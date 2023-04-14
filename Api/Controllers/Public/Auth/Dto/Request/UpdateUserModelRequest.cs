using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Request;

public class UpdateUserModelRequest
{
    [Required]
    [JsonProperty("Name")]
    public required string Name { get; init; }
    
    [Required]
    [EmailAddress]
    [JsonProperty("Email")]
    public required string Email { get; init; }
    
    [Required]
    [DataType(DataType.Password)]
    [JsonProperty("CurrentPassword")]
    public required string CurrentPassword { get; init; }
    
    [Required]
    [DataType(DataType.Password)]
    [JsonProperty("NewPassword")]
    public required string NewPassword { get; init; }
}