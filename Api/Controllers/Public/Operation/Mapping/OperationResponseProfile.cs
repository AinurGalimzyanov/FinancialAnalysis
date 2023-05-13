using AutoMapper;
using Dal.Operation.Entity;
using Logic.Managers.Operation.Dto.Response;

namespace Api.Controllers.Public.Operation.Mapping;

public class OperationResponseProfile : Profile
{
    public OperationResponseProfile()
    {
        CreateMap<OperationDal, OperationResponse>()
            .ForMember(dst => dst.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
            //.ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.CategoriesDal.Name))
            ;
    }
}