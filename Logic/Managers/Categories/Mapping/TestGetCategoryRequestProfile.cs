using AutoMapper;
using Dal.Categories.Entity;
using Logic.Managers.Categories.Dto.Response;

namespace Logic.Managers.Categories.Mapping;

/*public class TestGetCategoryRequestProfile : Profile
{
    public TestGetCategoryRequestProfile()
    {
        CreateMap<CategoriesDal, GetCategoryModelResponse>()
            .ForMember(dst => dst.name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dst => dst.Sum, opt => opt.MapFrom(src => src.OperationList.Select(x => x.Price).Sum()));
    }
}*/