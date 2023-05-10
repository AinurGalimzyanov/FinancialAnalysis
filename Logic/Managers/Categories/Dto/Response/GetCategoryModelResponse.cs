using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Categories.Dto.Response;

public class GetCategoryModelResponse
{
    [Required] 
    [JsonProperty("Name")] 
    public string name { get; init; }
    
    [Required] 
    [JsonProperty("Id")] 
    public Guid Id { get; init; }
    
    [Required] 
    [JsonProperty("Type")] 
    public string Type { get; init; }
    
    [Required] 
    [JsonProperty("Sum")] 
    public int? Sum { get; init; }

    public GetCategoryModelResponse(string name, Guid id, string type, int? sum)
    {
        this.name = name;
        Id = id;
        Type = type;
        Sum = sum;  
    }
}