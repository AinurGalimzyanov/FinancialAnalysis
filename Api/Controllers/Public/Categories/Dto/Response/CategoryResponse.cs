using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class CategoryResponse
{
    [Required] 
    [JsonProperty("Name")] 
    public string Name { get; init; }
    
    [Required] 
    [JsonProperty("Id")] 
    public Guid Id { get; init; }
    
    [Required] 
    [JsonProperty("Type")] 
    public string Type { get; init; }

    [Required]
    [JsonProperty("Sum")] 
    public int? Sum { get; init; } = 0;

    public CategoryResponse(string name, Guid id, string type, int? sum)
    {
        Name = name;
        Id = id;
        Type = type;
        Sum = sum;
    }
}