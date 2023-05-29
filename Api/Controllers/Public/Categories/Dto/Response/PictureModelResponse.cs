using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Controllers.Public.Categories.Dto.Response;

public class PictureModelResponse
{
    [Required]
    [JsonProperty("Picture")]
    public IFormFile Picture { get; init; }

    public PictureModelResponse(IFormFile picture)
    {
        Picture = picture;
    }
}