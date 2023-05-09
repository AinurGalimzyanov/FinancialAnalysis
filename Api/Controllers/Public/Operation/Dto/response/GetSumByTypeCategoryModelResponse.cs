using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetSumByTypeCategoryModelResponse
{
    [Required]
    [JsonProperty("Sum")]
    public int? Sum { get; init; }

    public GetSumByTypeCategoryModelResponse(int? sum)
    {
        Sum = sum;
    }
}