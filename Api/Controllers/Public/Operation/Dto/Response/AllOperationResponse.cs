using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Operation.Dto.Response;

public class AllOperationResponse
{
    [Required] 
    [JsonProperty("Operations")] 
    public List<OperationResponse> Operations { get; init; }

    public AllOperationResponse(List<OperationResponse> operations)
    {
        Operations = operations;    
    }
}