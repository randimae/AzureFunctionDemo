using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<FunctionRequest, LegacySystemRequest>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Reference))
                .ReverseMap();
            
            CreateMap<LegacySystemResponse, FunctionResponse>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Success, opt =>
                {
                    opt.MapFrom(src => src.ErrorCode.Equals(0) ? true : false);
                });


        }
    }
}
