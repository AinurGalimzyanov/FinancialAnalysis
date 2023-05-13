using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Operation.Dto.request;

public class AllOperationModelRequest
{
    [Required]
    [JsonProperty("DateTime")]
    public required DateTime DateTime { get; init; }
    
    [JsonProperty("Quantity")]
    [DefaultValue(0)]
    public int Quantity { get; init; }
}