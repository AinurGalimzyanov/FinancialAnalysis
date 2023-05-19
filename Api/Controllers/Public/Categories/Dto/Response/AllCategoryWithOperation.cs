using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class AllCategoryWithOperation
{
    [Required] 
    [JsonProperty("ListCategory")] 
    public List<CategoryResponse> ListCategory { get; init; }

    public AllCategoryWithOperation(List<CategoryResponse> listCategory)
    {
        ListCategory = listCategory;
    }
}