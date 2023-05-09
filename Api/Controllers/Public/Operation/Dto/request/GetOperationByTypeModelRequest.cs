using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class GetOperationByTypeModelRequest
{
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }

    [Required]
    [JsonProperty("DateTime")]
    public required DateTime DateTime { get; init; }
}