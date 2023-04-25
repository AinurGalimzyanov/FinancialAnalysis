using System.ComponentModel.DataAnnotations;
using Api.Controllers.Public.Categories.Dto.Response;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetOperationsModelResponse
{
    [Required] 
    [JsonProperty("Operations")] 
    public List<GetOperationModelResponse> Operations { get; init; }

    public GetOperationsModelResponse(List<GetOperationModelResponse> operations)
    {
        Operations = operations;
    }
}