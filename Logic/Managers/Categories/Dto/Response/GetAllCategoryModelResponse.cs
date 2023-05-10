using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Logic.Managers.Categories.Dto.Response;

public class GetAllCategoryModelResponse
{
    [Required] 
    [JsonProperty("ListCategoriesByType")] 
    public List<GetCategoryModelResponse> ListCategoriesByType { get; init; }
    

    public GetAllCategoryModelResponse(List<GetCategoryModelResponse> listCategoriesByType)
    {
        ListCategoriesByType = listCategoriesByType;
    }
}