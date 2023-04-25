using System.ComponentModel.DataAnnotations;
using Dal.Categories.Entity;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class GetAllCategoryModelResponse
{
    [Required] 
    [JsonProperty("ListCategories")] 
    public List<GetCategoryModelResponse> ListCategories { get; init; }

    public GetAllCategoryModelResponse(List<GetCategoryModelResponse> listCategories)
    {
        ListCategories = listCategories;
    }
}