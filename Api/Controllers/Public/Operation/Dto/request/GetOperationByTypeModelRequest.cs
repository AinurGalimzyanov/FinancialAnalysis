using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class GetOperationByTypeModelRequest
{
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }
    
    [Required]
    [JsonProperty("CategoryId")]
    public required Guid CategoryId { get; init; }
}