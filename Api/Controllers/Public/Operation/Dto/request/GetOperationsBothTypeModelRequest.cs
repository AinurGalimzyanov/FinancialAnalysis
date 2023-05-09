using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class GetOperationsBothTypeModelRequest
{
    [Required]
    [JsonProperty("DateTime")]
    public required DateTime DateTime { get; init; }
}