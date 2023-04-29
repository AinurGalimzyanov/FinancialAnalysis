using System.ComponentModel.DataAnnotations;
using Dal.Categories.Entity;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

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
    

    public GetCategoryModelResponse(string name, Guid id, string type)
    {
        this.name = name;
        Id = id;
        Type = type;
    }
}