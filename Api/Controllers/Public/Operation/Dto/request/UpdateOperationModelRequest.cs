using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class UpdateOperationModelRequest
{
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }
    
    [Required]
    [JsonProperty("Price")]
    public required int Price { get; init; }
}