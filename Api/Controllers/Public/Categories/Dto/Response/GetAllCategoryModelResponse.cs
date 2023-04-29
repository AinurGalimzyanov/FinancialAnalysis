using System.ComponentModel.DataAnnotations;
using Dal.Categories.Entity;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class GetAllCategoryModelResponse
{
    [Required] 
    [JsonProperty("ListCategoriesIncome")] 
    public List<GetCategoryModelResponse> ListCategoriesIncome { get; init; }
    
    [Required] 
    [JsonProperty("ListCategoriesExpenses")] 
    public List<GetCategoryModelResponse> ListCategoriesExpenses { get; init; }

    public GetAllCategoryModelResponse(List<GetCategoryModelResponse> listCategoriesIncome, List<GetCategoryModelResponse> listCategoriesExpenses)
    {
        ListCategoriesIncome = listCategoriesIncome;
        ListCategoriesExpenses = listCategoriesExpenses;
    }
}