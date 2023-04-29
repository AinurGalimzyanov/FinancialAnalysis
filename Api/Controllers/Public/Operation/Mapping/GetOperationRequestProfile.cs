using Api.Controllers.Operation.Dto.response;
using AutoMapper;
using Dal.Operation.Entity;

namespace Api.Controllers.Operation.Mapping;

public class GetOperationRequestProfile : Profile
{
    public GetOperationRequestProfile()
    {
        CreateMap<OperationDal, GetOperationModelResponse>()
            .ForMember(dst => dst.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
            ;
    }
}