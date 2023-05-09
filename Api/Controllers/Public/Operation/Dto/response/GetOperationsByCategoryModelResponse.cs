using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetOperationsByCategoryModelResponse
{
    [Required] 
    [JsonProperty("Operations")] 
    public List<GetOperationModelResponse> Operations { get; init; }

    public GetOperationsByCategoryModelResponse(List<GetOperationModelResponse> operations)
    {
        Operations = operations;
    }
}