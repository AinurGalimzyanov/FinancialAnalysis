using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class CreateOperationModelRequest
{
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }
    
    [Required]
    [JsonProperty("Price")]
    public required int Price { get; init; }
    
    [Required]
    [JsonProperty("DateTime")]
    public required DateTime DateTime { get; init; }
    
    [Required]
    [JsonProperty("CategoryId")]
    public required Guid CategoryId { get; init; }
}