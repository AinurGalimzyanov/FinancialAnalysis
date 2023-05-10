using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetOperationModelResponse
{
    [Required]
    [JsonProperty("Id")]
    public Guid Id { get; init; }
    
    [Required]
    [JsonProperty("Price")]
    public int? Price { get; init; }
    
    [Required]
    [JsonProperty("DateTime")]
    public DateTime? DateTime { get; init; }
    
    [Required]
    [JsonProperty("Name")]
    public string? Name { get; init; }


    public GetOperationModelResponse(Guid id, int? price, DateTime? dateTime, string? name)
    {
        Id = id;
        Price = price;
        DateTime = dateTime;
        Name = name;
    }
}