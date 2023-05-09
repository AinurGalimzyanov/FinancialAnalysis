using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class GetSumByTypeModelRequest
{
    [Required]
    [JsonProperty("Type")]
    public required string Type { get; init; }
}