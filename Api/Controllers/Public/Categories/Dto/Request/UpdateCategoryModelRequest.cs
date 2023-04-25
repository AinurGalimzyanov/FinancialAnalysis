using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Request;

public class UpdateCategoryModelRequest
{
    [Required]
    [JsonProperty("Id")]
    public required Guid Id { get; init; }
    
    [Required]
    [JsonProperty("Name")]
    public required string Name { get; init; }
}