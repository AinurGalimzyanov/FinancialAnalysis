using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.request;

public class OperationsByCategoryModelRequest
{
    [Required]
    [JsonProperty("CategoryId")]
    public required Guid CategoryId { get; init; }
    
    [Required]
    [JsonProperty("Date")]
    public required DateTime Date { get; init; }
}