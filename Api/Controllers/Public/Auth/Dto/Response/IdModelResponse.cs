using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Auth.Dto.Response;

public class IdModelResponse
{
    [Required]
    [JsonProperty("Id")]
    public string Id;
    
    public IdModelResponse(string id)
    {
        Id = id;
    }
}