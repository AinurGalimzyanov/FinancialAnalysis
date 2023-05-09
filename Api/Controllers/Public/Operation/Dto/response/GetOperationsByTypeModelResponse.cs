using System.ComponentModel.DataAnnotations;
using Api.Controllers.Public.Categories.Dto.Response;
using Newtonsoft.Json;

namespace Api.Controllers.Operation.Dto.response;

public class GetOperationsByTypeModelResponse
{
    [Required] 
    [JsonProperty("OperationsIncome")] 
    public List<GetOperationModelResponse> OperationsIncome { get; init; }
    
    [Required] 
    [JsonProperty("OperationsExpenses")] 
    public List<GetOperationModelResponse> OperationsExpenses { get; init; }

    public GetOperationsByTypeModelResponse(List<GetOperationModelResponse> operationsIncome, List<GetOperationModelResponse> operationsExpenses)
    {
        OperationsIncome = operationsIncome;
        OperationsExpenses = operationsExpenses;
    }
}