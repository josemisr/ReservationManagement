using AutoMapper;
using DataAccess.Models;
using ServiceServiceApi.Models;

namespace ServiceServiceApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {      
            CreateMap<Services, ServiceDto>();
            CreateMap<ServiceDto, Services>();
        }
    }
}
