using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetLastFiveOperationsModelResponse
{
    [Required] 
    [JsonProperty("Operations")] 
    public List<GetOperationModelResponse> Operations{ get; init; }

    public GetLastFiveOperationsModelResponse(List<GetOperationModelResponse> operations)
    {
        Operations = operations;
    }
}