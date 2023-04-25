using Api.Controllers.Operation.Dto.request;
using AutoMapper;
using Dal.Operation.Entity;

namespace Api.Controllers.Operation.Mapping;

public class UpdateOperationProfile : Profile
{
    public UpdateOperationProfile()
    {
        CreateMap<UpdateOperationModelRequest, OperationDal>()
            .ForMember(dst => dst.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type));
    }
}