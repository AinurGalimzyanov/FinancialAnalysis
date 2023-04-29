using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Request;

public class CreateCategoriesModelRequest
{
    [Required]
    [JsonProperty("Name")]
    public required string Name { get; init; }
    
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }
}