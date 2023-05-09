using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class UpdateOperationModelRequest
{
    [Required]
    [JsonProperty("Id")]
    public required Guid Id { get; init; }

    [Required]
    [JsonProperty("Price")]
    public required int Price { get; init; }
    
    [Required]
    [JsonProperty("DateTime")]
    public required DateTime DateTime { get; init; }
}