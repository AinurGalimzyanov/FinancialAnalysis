using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetBalanceModelResponse
{
    [Required]
    [JsonProperty("Balance")]
    public int? Balance { get; init; }

    public GetBalanceModelResponse(int? balance)
    {
        Balance = balance;
    }
}