using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Operation.Dto.Response;

public class SumResponse
{
    [Required]
    [JsonProperty("Sum")]
    public int? Sum { get; init; }

    public SumResponse(int? sum)
    {
        Sum = sum;
    }
}