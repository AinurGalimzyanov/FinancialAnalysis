using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class AllCategoryWithOperation
{
    [Required] 
    [JsonProperty("ListCategoryIncome")] 
    public List<CategoryResponse> ListCategoryIncome { get; init; }
    
    [Required] 
    [JsonProperty("ListCategoryExpenses")] 
    public List<CategoryResponse> ListCategoryExpenses { get; init; }
    
    public AllCategoryWithOperation(List<CategoryResponse> listCategoryIncome, List<CategoryResponse> listCategoryExpenses)
    {
        ListCategoryIncome = listCategoryIncome;
        ListCategoryExpenses = listCategoryExpenses;
    }
}