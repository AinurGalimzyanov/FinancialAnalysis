using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Request;

public class SiginWithAccessModelRequest
{
    [Required]
    [JsonProperty("Access")]
    public required string Access { get; init; }
}