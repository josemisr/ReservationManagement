
using AutoMapper;
using DataAccess.Models;
using FunctionsService.Models;

namespace FunctionsService
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
