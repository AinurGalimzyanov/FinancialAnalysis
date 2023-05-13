using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Operation.Dto.Response;

public class OperationByTypeResponse
{
    [Required] 
    [JsonProperty("OperationsIncome")] 
    public List<OperationResponse> OperationsIncome { get; init; }
    
    [Required] 
    [JsonProperty("OperationsExpenses")] 
    public List<OperationResponse> OperationsExpenses { get; init; }

    public OperationByTypeResponse(List<OperationResponse> operationsIncome, List<OperationResponse> operationsExpenses)
    {
        OperationsIncome = operationsIncome;
        OperationsExpenses = operationsExpenses;
    }
}