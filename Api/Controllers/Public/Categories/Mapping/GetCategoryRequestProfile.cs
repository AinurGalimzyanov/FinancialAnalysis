using Api.Controllers.Public.Categories.Dto.Response;
using AutoMapper;
using Dal.Categories.Entity;

namespace Api.Controllers.Public.Categories.Mapping;

public class GetCategoryRequestProfile : Profile
{
    public GetCategoryRequestProfile()
    {
        CreateMap<Tuple<CategoriesDal, int?>, GetCategoryModelResponse>()
            .ForMember(dst => dst.name, opt => opt.MapFrom(src => src.Item1.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Item1.Id))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Item1.Type))
            .ForMember(dst => dst.Sum, opt => opt.MapFrom(src => src.Item2));
    }
}