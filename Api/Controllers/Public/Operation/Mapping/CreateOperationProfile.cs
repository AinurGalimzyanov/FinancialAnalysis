using Api.Controllers.Operation.Dto.request;
using Api.Controllers.Operation.Dto.response;
using AutoMapper;
using Dal.Operation.Entity;

namespace Api.Controllers.Operation.Mapping;

public class CreateOperationProfile : Profile
{
    public CreateOperationProfile()
    {
        CreateMap<GetOperationModelResponse, OperationDal>()
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dst => dst.DateTime, opt => opt.MapFrom(src => src.DateTime))
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id));
    }
    
}