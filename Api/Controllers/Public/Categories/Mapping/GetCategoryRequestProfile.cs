using Api.Controllers.Public.Categories.Dto.Response;
using AutoMapper;
using Dal.Categories.Entity;

namespace Api.Controllers.Public.Categories.Mapping;

public class GetCategoryRequestProfile : Profile
{
    public GetCategoryRequestProfile()
    {
        CreateMap<CategoriesDal, GetCategoryModelResponse>()
            .ForMember(dst => dst.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));
    }
}